using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    [BigTable]
    public class ItemsController : ApiController
    {
        // GET api/items
        public IEnumerable<string> Get(String type, [FromUri]int page)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/items/5
        public string Get(String uid)
        {
            return "value";
        }

        // POST api/items
        public void Post(String uid, HttpRequestMessage value, [FromUri]string type)
        {
            var json = value.Content.ReadAsStringAsync().Result;
        }

        // PUT api/items/5
        public void Put(String uid, [FromBody]string value)
        {
        }

        // DELETE api/items/5
        public void Delete(String uid)
        {
        }
    }
}
