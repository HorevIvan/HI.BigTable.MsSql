using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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
            var type = GetTypeName(item);

            var data = Serialize(item);

            Insert(item.UID, data, type);
        }

        public virtual void InsertFile(String name, Byte[] bytes)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"insert into Files (Name, Data) values (@Name, @Data)";

                connection.Execute(sql, new { Data = bytes, Name = name });
            }
        }

        public void Insert(String uid, String data, String type)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"insert into Items (UID, Type, Data) values (@UID, @Type, @Data)";

                connection.Execute(sql, new { UID = uid, Type = type, Data = data });
            }
        }

        public virtual void Update(Item item)
        {
            var data = Serialize(item);

            Update(item.UID, data);
        }

        public virtual void Update(String name, Byte[] bytes)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"update File set Data = @Data where Name = name";

                connection.Execute(sql, new { Data = bytes, Name = name });
            }
        }

        public void Update(String uid, String data)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"update Items set Data = @Data where UID = @UID";

                connection.Execute(sql, new { UID = uid, Data = data });
            }
        }

        public virtual void Delete(String itemUID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"delete Items where UID = @UID";

                connection.Execute(sql, new { UID = itemUID });
            }
        }

        public virtual void DeleteFile(String name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"delete Files where Name = @Name";

                connection.Execute(sql, new { Name = name });
            }
        }

        public virtual T Select<T>(String itemUID) where T : Item
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

        public virtual ItemRow Select(String itemUID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $"select * from Items where UID = @UID";

                var row = connection.QuerySingleOrDefault<ItemRow>(sql, new { UID = itemUID });

                return row;
            }
        }

        public virtual FileRow SelectFile(String name)
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
