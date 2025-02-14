using Dormate.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Dormate.Infrastructure.Repositories
{
    public interface IRoleRepository : IBaseRepository<IdentityRole, string>
    {

    }
    public class RoleRepository : BaseRepository<IdentityRole, string>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
