namespace DotNetCore.DataLayer.Dapper
{
    public interface IDatabaseConnectionSettings
    {
        string Host { get; }

        uint Port { get; }

        string DatabaseName { get; }

        string User { get; }

        string Password { get; }

        bool Pooling { get; }
    }
}
