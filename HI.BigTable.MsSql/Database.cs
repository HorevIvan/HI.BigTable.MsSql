using Dapper;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task<SqlConnection> GetConnectionAsync(bool open = true)
        {
            var connection = new SqlConnection(ConnectionString);

            if (open) await connection.OpenAsync();

            return connection;
        }

        public async Task CreateStructureAsync()
        {
            using (var connection = await GetConnectionAsync())
            {
                var itemsTable = $"create table Items(DBID bigint identity not null primary key,UID {ItemsKey} not null unique,Type varchar(50) not null index IX_Items_Type,Data nvarchar(max) not null)";

                await connection.ExecuteAsync(itemsTable);

                var filesTable = $"create table Files(DBID int identity not null primary key,Name {FilesKey} not null unique,Data varbinary(max) not null)";

                await connection.ExecuteAsync(filesTable);
            }
        }

        public static async Task<Database> CreateAsync(String connectionString, String directory, String name)
        {
            var path = System.IO.Path.Combine(directory, name);

            using (var connection = new SqlConnection(connectionString))
            {
                var db = $"CREATE DATABASE [{name}] CONTAINMENT = NONE ON PRIMARY ( NAME = N'{name}', FILENAME = N'{path}.mdf', SIZE = 8192KB, MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB) LOG ON  (NAME = N'DATABASE_log', FILENAME = N'{path}_log.ldf', SIZE = 8192KB, MAXSIZE = 2048GB, FILEGROWTH = 65536KB)";

                await connection.ExecuteAsync(db);
            }

            var database = GetDatabase(connectionString, name);

            await database.CreateStructureAsync();

            return database;
        }

        public static Database GetDatabase(String connectionString, String databaseName)
        {
            return new Database(connectionString + ";Initial Catalog=" + databaseName);
        }

        public static async Task<Boolean> ExistsAsync(String connectionString, String databaseName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbID = await GetDatabaseID(connection, databaseName);

                return dbID.HasValue;
            }
        }

        public static async Task<Int32?> GetDatabaseID(SqlConnection connection, String databaseName)
        {
            var sql = "select DB_ID(@DatabaseName)";

            var dbID = await connection.ExecuteScalarAsync<int?>(sql, new { DatabaseName = databaseName });

            return dbID;
        }

        public DataContext GetDataContext()
        {
            return new DataContext(ConnectionString);
        }
    }
}
