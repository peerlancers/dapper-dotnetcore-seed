namespace DotNetCore.DataLayer
{
    public interface IPagingInfo
    {
        int Page { get; }

        int PerPage { get; }

        string ToQueryString();
    }
}
