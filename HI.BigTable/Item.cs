using Newtonsoft.Json;
using System;

namespace HI.BigTable
{
    public abstract class Item
    {
        [JsonIgnore]
        public String UID { set; get; }
    }
}
