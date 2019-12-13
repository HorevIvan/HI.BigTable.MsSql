using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    public class BigTableAttribute : ActionFilterAttribute
    {
        public static IDictionary<string, Database> Databases { get; } = new SortedList<string, Database>();

        public static String ConnectionString { get; set; }

        public static String DatabaseDirectory { get; set; }

        public static String DatabaseName { get; set; }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var databaseName = DatabaseName ?? GetDatabaseName(actionContext.Request.RequestUri.Host);

            if (!Databases.ContainsKey(databaseName))
            {
                var database = await GetDatabase(databaseName);

                lock (Databases)
                {
                    Databases[databaseName] = database;
                }
            }

            if (actionContext.ControllerContext.Controller is BigTableController itemsController)
            {
                itemsController.DataContext = Databases[databaseName].GetDataContext();
            }

            return;
        }

        private static async Task<Database> GetDatabase(String databaseName)
        {
            if (await Database.ExistsAsync(ConnectionString, databaseName))
            {
                return Database.GetDatabase(ConnectionString, databaseName);
            }
            else
            {
                return await Database.CreateAsync(ConnectionString, DatabaseDirectory, databaseName);
            }
        }

        private String GetDatabaseName(String host)
        {
            return host.Split('.').Reverse().Aggregate("", (a, i) => a += i + ".").TrimEnd('.');
        }
    }
}
