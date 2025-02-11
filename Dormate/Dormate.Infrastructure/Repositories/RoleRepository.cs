using Dormate.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Dormate.Infrastructure.Repositories
{
    public interface IRoleRepository : IBaseRepository<IdentityRole, String>
    {

    }
    public class RoleRepository : BaseRepository<IdentityRole, String>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
