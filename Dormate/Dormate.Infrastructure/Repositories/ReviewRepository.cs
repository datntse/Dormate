using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review, string> 
    {
    }
    public class ReviewRepository : BaseRepository<Review, string>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }


}
