namespace Dormate.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
	}
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _applicationDbContext;
		public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
		public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
		{
            return (await _applicationDbContext.SaveChangesAsync()) > 0;
        }
	}
}
