using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public HttpResponseMessage Get(String uid)
        {
            var json = DataContext.Select(uid).Data;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
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
