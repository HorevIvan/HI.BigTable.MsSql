using System;

namespace HI.BigTable.MsSql
{
    public class FileRow
    {
        public Guid UID { set; get; }

        public String Name { set; get; }

        public String Type { set; get; }

        public Byte[] Data { set; get; }
    }
}
