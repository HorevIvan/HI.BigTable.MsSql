using Dapper;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;

namespace HI.BigTable.MsSql
{
    public class DataContext
    {
        public String ConnectionString { get; }

        public DataContext(String connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual void Insert(Item item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"insert into Items (UID, Type, Data) values (@UID, @Type, @Data)";

                var type = GetTypeName(item);

                var data = Serialize(item);

                connection.Execute(sql, new { UID = item.UID, Type = type, Data = data });
            }
        }

        public virtual void Insert(String name, Byte[] bytes)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"insert into Files (Name, Data) values (@Name, @Data)";

                connection.Execute(sql, new { Data = bytes, Name = name });
            }
        }

        public virtual void Update(Item item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"update Items set Data = @Data where UID = @UID";

                var data = Serialize(item);

                connection.Execute(sql, new { UID = item.UID, Data = data });
            }
        }

        public virtual void Update(String name, Byte[] bytes)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"update File set Data = @Data where Name = name";

                connection.Execute(sql, new { Data = bytes, Name = name });
            }
        }

        public virtual void Delete(Guid itemUID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"delete Items where UID = @UID";

                connection.Execute(sql, new { UID = itemUID });
            }
        }

        public virtual void Delete(String name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"delete Files where Name = @Name";

                connection.Execute(sql, new { Name = name });
            }
        }

        public virtual T Select<T>(Guid itemUID) where T : Item
        {
            var row = Select(itemUID);

            if (row == null) return null;

            return Deserialize<T>(row);
        }

        private static T Deserialize<T>(ItemRow row) where T : Item
        {
            var item = JsonConvert.DeserializeObject<T>(row.Data);

            item.UID = row.UID;

            return item;
        }

        public virtual ItemRow Select(Guid itemUID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"select * from Items where UID = @UID";

                var row = connection.QuerySingleOrDefault<ItemRow>(sql, new { UID = itemUID });

                return row;
            }
        }

        public virtual FileRow Select(String name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"select * from Files where Name = @Name";

                var row = connection.QuerySingleOrDefault<FileRow>(sql, new { Name = name });

                return row;
            }
        }

        private static String Serialize(Item item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private static String GetTypeName(Item item)
        {
            return item.GetType().Name;
        }
    }
}
