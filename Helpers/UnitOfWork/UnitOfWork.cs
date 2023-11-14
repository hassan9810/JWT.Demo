using JWT.Demo.Context;
using JWT.Demo.Helpers.GenericRepositories;
using JWT.Demo.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace JWT.Demo.Helpers.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        #region Repository
        public IGRepository<Employee> _employeeRepository { get; }

        #endregion

        #region constructor
        public UnitOfWork
        (
            AppDbContext context,
            IGRepository<Employee> employeeRepository
        )
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Transaction
        public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();
        //public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }
        public void Commit() => _transaction.Commit();
        //public async Task CommitAsync() => await _transaction.CommitAsync();
        public async Task CommitAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _transaction = null;
                }
            }
            catch (Exception)
            {
                await RollbackAsync();
                await Console.Out.WriteLineAsync("RollBack!");
                throw new InvalidOperationException("RollBack!");
            }
        }
        public void Rollback() => _transaction?.Rollback();
        //public async Task RollbackAsync() => await _transaction.RollbackAsync();
        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _transaction = null;
                }
                else
                {
                    await Console.Out.WriteLineAsync("NoActiveTransactionToRollBack!");
                    throw new InvalidOperationException("NoActiveTransactionToRollBack!");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message + (ex != null && ex.InnerException != null ? ex.InnerException.Message : ""));
                throw new InvalidOperationException("anErrorOccurredPleaseContactSystemAdministrator!");
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        public async Task DisposeAsync()
        {
            await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
        #endregion

        #region SaveChanges
        public void SaveChanges()
        {
            try
            {
                if (_context.Database.CurrentTransaction == null)
                    BeginTransaction();
                _context.SaveChanges();
                //Commit();
            }
            catch (Exception)
            {
                Rollback();

            }

        }

        public async Task SaveChangesAsync()
        {
            try
            {
                if (_context.Database.CurrentTransaction == null)
                    await BeginTransactionAsync();
                await _context.SaveChangesAsync();
                //Commit();
            }
            catch (Exception)
            {
                await RollbackAsync();

            }

        }
        #endregion

    }
}
