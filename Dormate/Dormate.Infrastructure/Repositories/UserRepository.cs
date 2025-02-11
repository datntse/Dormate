
using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;

namespace Dormate.Infrastructure.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser, string>
    {

    }
    public class UserRepository : BaseRepository<ApplicationUser, string>, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
