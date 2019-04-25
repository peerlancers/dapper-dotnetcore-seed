namespace DotNetCore.DataLayer.Dapper
{
    public class PostgresPaging : IPagingInfo
    {
        private int page;

        private int perPage;

        public PostgresPaging(int page, int perPage)
        {
            Page = page;
            PerPage = perPage;
        }

        /// <summary>
        /// Defaults to 1 if value is less than 1
        /// </summary>
        public int Page
        {

            get
            {
                return page;
            }

            set => page = value <= 0 ? 1 : value;
        }

        /// <summary>
        /// Defaults to allowed maximum record per page if set to less than 1
        /// </summary>
        public int PerPage
        {
            get
            {
                return perPage;
            }

            set => perPage = value <= 0 ? 300 : value;
        }

        public string ToQueryString()
        {
            return $"LIMIT {PerPage} OFFSET {(Page - 1) * PerPage}";
        }
    }
}
