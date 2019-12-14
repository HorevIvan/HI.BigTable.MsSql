using HI.BigTable.MsSql.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HI.BigTable.MsSql.WebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BigTableAttribute.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            BigTableAttribute.DatabaseDirectory = ConfigurationManager.AppSettings["DatabaseDirectory"];
            BigTableAttribute.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];

            RegisterApis(GlobalConfiguration.Configuration);
        }

        public static void RegisterApis(HttpConfiguration config)
        {
            var matches = config.Formatters
                .Where(f => f.SupportedMediaTypes.Where(m => m.MediaType.ToString() == "application/xml" || m.MediaType.ToString() == "text/xml").Count() > 0)
                .ToList();

            foreach (var match in matches) config.Formatters.Remove(match);
        }
    }
}
