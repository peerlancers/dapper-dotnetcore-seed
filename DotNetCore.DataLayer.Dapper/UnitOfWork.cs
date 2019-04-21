using DotNetCore.DataLayer.Dapper.Repositories;
using DotNetCore.DataLayer.Interfaces;
using System;
using System.Data;

namespace DotNetCore.DataLayer.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        public IDbFactory Db { get; private set; }

        public IDbTransaction DbTransaction { get; private set; }

        private IUserRepository _userRepository;
        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(Db, DbTransaction));

        private ICompanyRepository _companyRepository;
        public ICompanyRepository Companies => _companyRepository ?? (_companyRepository = new CompanyRepository(Db, DbTransaction));

        public UnitOfWork(IDbFactory dbFactory)
        {
            Db = dbFactory;
            if (Db.Context()?.State == ConnectionState.Closed) {
                Db.Context().Open();
            }
            DbTransaction = Db.Context().BeginTransaction();
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
                DbTransaction = Db.Context().BeginTransaction();
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

                if (Db != null)
                {
                    Db.Dispose();
                    Db = null;
                }

                disposed = true;
            }
        }

        private void ResetRepositories()
        {

        }
    }
}
