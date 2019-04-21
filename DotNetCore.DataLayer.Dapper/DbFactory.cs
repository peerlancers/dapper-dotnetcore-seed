using System;
using System.Data;

namespace DotNetCore.DataLayer.Dapper
{
    public abstract class DbFactory : IDbFactory
    {
        private bool disposed = false;
        private readonly string connectionString;
        private IDbConnection _dbContext;

        public DbFactory()
        {
        }

        public DbFactory(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _dbContext = Connection(this.connectionString);
        }

        public abstract IDbConnection Connection(string connectionString);

        public virtual IDbConnection Context()
        {
            return _dbContext ?? (_dbContext = Connection(connectionString));
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
                _dbContext?.Dispose();
                _dbContext = null;
                disposed = true;
            }
        }
    }
}
