using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    public class ItemsController : BigTableController
    {
        // GET api/items
        public JsonResult<Item[]> Get([FromUri]String type, [FromUri]int page)
        {
            return Json(DataContext.SelectPage(page, type));
        }

        // GET api/items/5
        public string Get(String uid)
        {
            return DataContext.Select(uid).Data;
        }

        // POST api/items
        public async Task Post(String uid, HttpRequestMessage value, [FromUri]string type)
        {
            var data = await value.Content.ReadAsStringAsync();

            DataContext.Insert(uid, data, type);
        }

        // PUT api/items/5
        public void Put(String uid, HttpRequestMessage value)
        {
            var data = value.Content.ReadAsStringAsync().Result;

            DataContext.Update(uid, data);
        }

        // DELETE api/items/5
        public void Delete(String uid)
        {
            DataContext.Delete(uid);
        }
    }
}
