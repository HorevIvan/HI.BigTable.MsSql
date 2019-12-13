using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HI.BigTable.MsSql.WebApp.Controllers
{
    public class ItemsController : BigTableController
    {
        // GET api/items
        public IEnumerable<string> Get(String type, [FromUri]int page)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/items/5
        public string Get(String uid)
        {
            return DataContext.Select(uid).Data;
        }

        // POST api/items
        public void Post(String uid, HttpRequestMessage value, [FromUri]string type)
        {
            var json = value.Content.ReadAsStringAsync().Result;

            DataContext.Insert(uid, json, type);
        }

        // PUT api/items/5
        public void Put(String uid, HttpRequestMessage value)
        {
            var json = value.Content.ReadAsStringAsync().Result;

            DataContext.Update(uid, json);
        }

        // DELETE api/items/5
        public void Delete(String uid)
        {
            DataContext.Delete(uid);
        }
    }
}
