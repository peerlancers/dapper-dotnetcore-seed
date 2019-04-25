using System.Collections.Generic;

namespace DotNetCore.DataLayer
{
    public class DbRecordList<TRecord>
    {
        public DbRecordList(IPagingInfo paging)
        {
            Paging = paging;
        }

        public IPagingInfo Paging { get; private set; }

        public long TotalCount { get; set; }

        public List<TRecord> Records { get; set; }
    }
}
