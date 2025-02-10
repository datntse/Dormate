using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Entities
{
    public class RoomImage
    {
        [Key] 
        public string Id { get;set; }
        public string ImageUrl { get; set; }
        [ForeignKey(nameof(Room))]
        public string RoomId { get; set; }
        public Room Room { get; set; }
    }
}
