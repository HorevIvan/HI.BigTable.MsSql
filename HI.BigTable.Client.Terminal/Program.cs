using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HI.BigTable.Client.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new BigTableClient("http://localhost:50327");

            var user = new User { UID = Guid.NewGuid().ToString(), FirstName = "Ivan", LastName = "Horev" };

            await client.Insert(user);

            user = await client.Get<User>(user.UID);

            ;
        }
    }

    public class User : Item
    {
        public string FirstName { set; get; }

        public string LastName { set; get; }
    }
}
