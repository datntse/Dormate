using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{

    public interface IRoomRegisterRepository : IBaseRepository<RoomRegister, string>
    {

    }
    public class RoomRegisterRepository : BaseRepository<RoomRegister, string>, IRoomRegisterRepository
    {
        public RoomRegisterRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
