using DotNetCore.DataLayer.Dapper.Repositories;
using DotNetCore.DataLayer.Interfaces;
using System;
using System.Data;

namespace DotNetCore.DataLayer.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        public IDbFactory DbFactory { get; private set; }

        public IDapperDbClient DbClient { get; private set; }

        public IDbTransaction DbTransaction { get; private set; }

        private IUserRepository userRepository;
        public IUserRepository Users => userRepository ?? (userRepository = new UserRepository(DbClient));

        private ICompanyRepository companyRepository;
        public ICompanyRepository Companies => companyRepository ?? (companyRepository = new CompanyRepository(DbClient));

        public UnitOfWork(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            if (DbFactory.Context()?.State == ConnectionState.Closed) {
                DbFactory.Context().Open();
            }
            DbTransaction = DbFactory.Context().BeginTransaction();
            DbClient = new DapperDbClient(DbFactory, DbTransaction);
        }

        public void Save()
        {
            try
            {
                DbTransaction.Commit();
            }
            catch
            {
                DbTransaction.Rollback();
                throw;
            }
            finally
            {
                DbTransaction.Dispose();
                DbTransaction = DbFactory.Context().BeginTransaction();
                ResetRepositories();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Dispose();
                    DbTransaction = null;
                }

                if (DbFactory != null)
                {
                    DbFactory.Dispose();
                    DbFactory = null;
                }

                disposed = true;
            }
        }

        private void ResetRepositories()
        {

        }
    }
}
