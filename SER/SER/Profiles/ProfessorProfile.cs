using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class ProfessorProfile : Profile
  {
    public ProfessorProfile()
    {
      CreateMap<Professor, ProfessorDto>()
        .ReverseMap();
    }
  }
}