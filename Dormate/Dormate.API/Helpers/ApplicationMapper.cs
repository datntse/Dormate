using AutoMapper;
using Dormate.Core.Entities;
using Dormate.Core.Models;


namespace Dormate.API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            #region User
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserCM>().ReverseMap();
            CreateMap<ApplicationUser, UserRolesVM>().ReverseMap();
            #endregion


        }
    }
}
