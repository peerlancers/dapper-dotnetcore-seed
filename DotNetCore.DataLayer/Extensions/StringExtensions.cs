using System.Linq;

namespace DotNetCore.DataLayer.Extensions
{
    public static class StringExtensions
    {
        public enum CaseType
        {
            SnakeCase,
            CamelCase
        }

        public static string PascalCaseTo(this string str, CaseType type)
        {
            switch (type)
            {
                case CaseType.SnakeCase:
                    return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();

                case CaseType.CamelCase:
                    return str.Substring(0, 1).ToLower() + str.Substring(1);

                default:
                    return str;
            }
        }
    }
}
