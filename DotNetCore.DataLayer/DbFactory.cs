using System;
using System.Data;

namespace DotNetCore.DataLayer
{
    public abstract class DbFactory : IDbFactory
    {
        private bool disposed = false;
        private readonly string connectionString;
        private IDbConnection dbContext;

        public DbFactory(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            dbContext = Connection(this.connectionString);
        }

        public abstract IDbConnection Connection(string connectionString);

        public virtual IDbConnection Context()
        {
            return dbContext ?? (dbContext = Connection(connectionString));
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                dbContext?.Dispose();
                dbContext = null;
                disposed = true;
            }
        }
    }
}
