using AutoMapper;
using Seed.Models.V1.DTOs;
using Seed.Models.V1.Models;

namespace Seed.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        /// <summary>
        /// Create a MapperConfiguration instance and initialize configuration via the constructor:
        /// </summary>
        public AutoMapperProfiles()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<User, UserRequestDto>().ReverseMap();
            CreateMap<User, UserToSignInDto>().ReverseMap();
        }
    }
}
