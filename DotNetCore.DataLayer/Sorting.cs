using System;

namespace DotNetCore.DataLayer
{
    public enum SortDirection
    {
        Asc,
        Desc
    }

    public class Sorting<TFields> where TFields : struct, IConvertible
    {
        private TFields sortField;
        private SortDirection sortDirection;

        public Sorting(TFields sortField, SortDirection sortDirection = SortDirection.Asc)
        {
            SortField = sortField;
            SortDirection = sortDirection;
        }

        public TFields SortField
        {
            get
            {
                return sortField;
            }

            set => sortField = IsValid(value) ? value : default;
        }

        public SortDirection SortDirection
        {
            get
            {
                return sortDirection;
            }

            set => sortDirection = IsValid(value) ? value : default;
        }

        private static bool IsValid<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            var enumValue = Convert.ChangeType(value, typeof(TEnum));

            if (enumValue != null)
            {
                return Enum.IsDefined(typeof(TEnum), enumValue);
            }

            return true;
        }
    }
}
