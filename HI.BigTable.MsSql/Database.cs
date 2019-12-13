using Dapper;
using System;
using System.Data.SqlClient;

namespace HI.BigTable.MsSql
{
    public class Database
    {
        public String ConnectionString { get; }

        public String ItemsKey { set; get; } = "varchar(50)";

        public String FilesKey { set; get; } = "varchar(850)";

        public Database(String connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection GetConnection(bool open = true)
        {
            var connection = new SqlConnection(ConnectionString);

            if (open) connection.Open();

            return connection;
        }

        public void CreateStructure()
        {
            using (var connection = GetConnection())
            {
                var itemsTable = $"create table Items(DBID bigint identity not null primary key,UID {ItemsKey} not null unique,Type varchar(50) not null index IX_Items_Type,Data nvarchar(max) not null)";

                connection.Execute(itemsTable);

                var filesTable = $"create table Files(DBID int identity not null primary key,Name {FilesKey} not null unique,Data varbinary(max) not null)";

                connection.Execute(filesTable);
            }
        }

        public static Database Create(String connectionString, String directory, String name)
        {
            var path = System.IO.Path.Combine(directory, name);

            using (var connection = new SqlConnection(connectionString))
            {
                var db = $"CREATE DATABASE [{name}] CONTAINMENT = NONE ON PRIMARY ( NAME = N'{name}', FILENAME = N'{path}.mdf', SIZE = 8192KB, MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB) LOG ON  (NAME = N'DATABASE_log', FILENAME = N'{path}_log.ldf', SIZE = 8192KB, MAXSIZE = 2048GB, FILEGROWTH = 65536KB)";

                connection.Execute(db);
            }

            var databse = new Database(connectionString + ";Initial Catalog=" + name);

            databse.CreateStructure();

            return databse;
        }

        public DataContext GetDataContext()
        {
            return new DataContext(ConnectionString);
        }
    }
}
