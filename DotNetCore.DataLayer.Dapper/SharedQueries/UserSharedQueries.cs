namespace DotNetCore.DataLayer.Dapper
{
    public static partial class SharedQueries
    {
        public static class Users
        {
            public static string GetUserById(string idParamName)
            {
                return $@"SELECT * FROM {TableNames.Users} WHERE id = @{idParamName};";
            }
        }
    }
}
