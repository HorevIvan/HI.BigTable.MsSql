using System;
using System.Collections.Generic;
using System.Text;

namespace HI.BigTable.MsSql
{
    public class ItemRow
    {
        public Guid UID { set; get; }

        public String Type { set; get; }

        public String Data { set; get; }
    }
}
