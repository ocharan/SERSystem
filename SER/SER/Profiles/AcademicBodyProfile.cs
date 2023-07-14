using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;
namespace SER.Profiles
{
  public class AcademicBodyProfile : Profile
  {
    public AcademicBodyProfile()
    {
      CreateMap<AcademicBody, AcademicBodyDto>().ReverseMap();
      CreateMap<AcademicBodyMember, AcademicBodyMemberDto>().ReverseMap();
    }
  }
}