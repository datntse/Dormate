using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Entities
{
    public class Review
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedById { get; set; }
        public string RoomId { get; set; }
        public int? Rate { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
