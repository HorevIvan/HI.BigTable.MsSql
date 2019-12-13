using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    public class ItemsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get(String type, [FromUri]int page)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(String uid)
        {
            return "value";
        }

        // POST api/values
        public void Post(String uid, HttpRequestMessage value, [FromUri]string type)
        {
            var json = value.Content.ReadAsStringAsync().Result;
        }

        // PUT api/values/5
        public void Put(String uid, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(String uid)
        {
        }
    }
}
