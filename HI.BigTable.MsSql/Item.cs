using Newtonsoft.Json;
using System;

namespace HI.BigTable.MsSql
{
    public abstract class Item
    {
        [JsonIgnore]
        public String UID { set; get; }
    }
}
