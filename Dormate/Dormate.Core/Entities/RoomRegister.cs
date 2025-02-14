using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Entities
{
    public class RoomRegister
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey(nameof(Room))]
        public string RoomId { get;set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<ApplicationUser> Tenants { get; set; }
    }
}
