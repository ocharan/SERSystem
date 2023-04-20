using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<User, UserDto>().ReverseMap();
    }
  }
}