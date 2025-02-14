using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{
    public interface IRoomImageRepository : IBaseRepository<RoomImage, string>
    {

    }
    public class RoomImageRepository : BaseRepository<RoomImage, string>, IRoomImageRepository
    {
        public RoomImageRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
