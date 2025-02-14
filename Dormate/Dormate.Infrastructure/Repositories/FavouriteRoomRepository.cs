using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{
    public interface IFavouriteRoomRepository : IBaseRepository<FavouriteRoom, string>
    {

    }
    public class FavouriteRoomRepository : BaseRepository<FavouriteRoom, string>, IFavouriteRoomRepository
    {
        public FavouriteRoomRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
