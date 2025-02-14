using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Entities
{
    public class FavouriteRoom
    {
        [Key]
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<ApplicationUser> Tenant { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}
