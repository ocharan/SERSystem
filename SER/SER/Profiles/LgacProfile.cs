using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class LgacProfile : Profile
  {
    public LgacProfile()
    {
      CreateMap<Lgac, LgacDto>().ReverseMap();
      CreateMap<AcademicBodyLgac, AcademicBodyLgacDto>().ReverseMap();
    }
  }
}