using Dormate.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Models
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? DOB { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; } = 1;
        public int Type { get; set; } = 1;
    }

    public class UserRolesVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public int Status { get; set; } = 1;
        public List<string> RolesName { get; set; }

    }

    public class UserRoles : ApplicationUser
    {
        public List<string> RolesName { get; set; }
    }


    public class UserCM : UserDTO
    {
        public string? LoginProvider { get; set; }
        public string? ProviderDisplayName { get; set; }
        public string? ProviderKey { get; set; }
        public string RoleName { get; set; }
    }

    public class UserSignIn
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }

    public class UserSignInVM : UserSignIn
    {
        public string FullName { get; set; }
        public string Email { get; set; }
    }


   

}
