using JWT.Demo.Helpers.GenericRepositories;
using JWT.Demo.Models.Entities;

namespace JWT.Demo.Helpers.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        #region Repository

        public IGRepository<Employee> _employeeRepository { get; }

        #endregion

        #region Transaction
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        #endregion

        #region SaveChanges
        Task SaveChangesAsync();
        void SaveChanges();
        #endregion
    }
}
