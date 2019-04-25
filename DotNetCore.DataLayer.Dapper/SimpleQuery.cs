using DotNetCore.DataLayer.Extensions;
using System;

namespace DotNetCore.DataLayer.Dapper
{
    public class SimpleQuery
    {
        private readonly string query;
        private string filter;
        private string sorting;
        private string paging;

        public SimpleQuery(string query)
        {
            this.query = query;
        }

        public SimpleQuery SetFilter(string filter)
        {
            this.filter = "";
            if (!string.IsNullOrEmpty(filter))
            {
                this.filter = filter;
            }

            return this;
        }

        public SimpleQuery SortBy<TSortableFields>(Sorting<TSortableFields> sorting = null) where TSortableFields : struct, IConvertible
        {
            if (sorting != null) {
                this.sorting = $"ORDER BY {sorting.SortField.ToString().ToSnakeCase()} {sorting.SortDirection.ToString()}";
            }

            return this;
        }

        public SimpleQuery Page(IPagingInfo paging = null)
        {
            this.paging = string.Empty;
            if (paging != null) {
                this.paging = paging.ToQueryString();
            };

            return this;
        }

        public new string ToString()
        {
            return $"{query} {filter} {sorting} {paging};";
        }
    }
}
