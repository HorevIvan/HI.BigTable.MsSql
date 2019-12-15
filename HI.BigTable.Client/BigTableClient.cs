using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HI.BigTable
{
    public class BigTableClient
    {
        public String Domain { get; }

        public BigTableClient(String domain)
        {
            Domain = domain.TrimEnd('/');
        }

        public async Task Insert<T>(T item) where T : Item
        {
            var path = $"{Domain}/api/items/{item.UID}?type={item.GetType().Name}";

            var request = WebRequest.Create(path);

            request.Method = "POST";

            request.ContentType = "application/json";

            var json = JsonConvert.SerializeObject(item);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            request.GetResponse();
        }

        public async Task<String> Get(String uid)
        {
            var path = $"{Domain}/api/items/{uid}";

            var request = WebRequest.Create(path);

            request.Method = "GET";

            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<T> Get<T>(String uid) where T : Item
        {
            var json = await Get(uid);

            var item = JsonConvert.DeserializeObject<T>(json);

            item.UID = uid;

            return item;
        }
    }
}
