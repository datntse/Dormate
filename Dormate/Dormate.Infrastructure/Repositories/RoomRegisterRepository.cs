using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{

    public interface IRoomRegisterRepository : IBaseRepository<Room, string>
    {

    }
    public class RoomRegisterRepository : BaseRepository<Room, string>, IRoomRegisterRepository
    {
        public RoomRegisterRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
