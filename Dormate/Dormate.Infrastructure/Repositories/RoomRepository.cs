using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room, string>
    {

    }
    public class RoomRepository : BaseRepository<Room, string>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
