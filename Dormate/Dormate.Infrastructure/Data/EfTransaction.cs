using Microsoft.EntityFrameworkCore.Storage;

namespace Dormate.Infrastructure.Data
{
	public class EfTransaction : IEfTransaction
	{
		private IDbContextTransaction _dbContextTransaction;

        public EfTransaction(ApplicationDbContext _appDbContext)
		{
			_dbContextTransaction = _appDbContext.Database.BeginTransaction();
		}

		public Task CommitAsync() => _dbContextTransaction.CommitAsync();

		public Task RollbackAsync() => _dbContextTransaction.RollbackAsync();

		public void Dispose()
		{
			_dbContextTransaction?.Dispose();
			_dbContextTransaction = null!;
		}

        public async Task CreateSavePoint(string point)
        {
            await _dbContextTransaction.CreateSavepointAsync(point);
        }
    }

	public interface IEfTransaction
    {
		Task CommitAsync();
		Task RollbackAsync();
		Task CreateSavePoint(string point);
		public void Dispose();

	}
}
