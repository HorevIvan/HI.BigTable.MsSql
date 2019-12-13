using System.Web.Http;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    [BigTable]
    public class BigTableController : ApiController
    {
        public DataContext DataContext { set; get; }
    }
}