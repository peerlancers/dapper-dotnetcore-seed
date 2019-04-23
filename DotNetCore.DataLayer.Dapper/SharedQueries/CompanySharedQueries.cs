namespace DotNetCore.DataLayer.Dapper
{
    public static partial class SharedQueries
    {
        public static class Companies
        {
            public static string GetCompanyById(string idParamName)
            {
                return $@"SELECT * FROM {TableNames.Companies} WHERE id = @{idParamName};";
            }
        }
    }
}
