using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace Dormate.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public string FullName { get; set; }
        public DateTime? DOB { get; set; }
        public string? PhoneNumber { get; set; }
        public int Status { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public String? RefreshToken { get; set; }
        public DateTime? LimitCreditPurchaseTime { get; set; }
        public DateTime? DateExpireRefreshToken { get; set; }
        public virtual ICollection<Room>? Rooms { get; set; }


    }
}