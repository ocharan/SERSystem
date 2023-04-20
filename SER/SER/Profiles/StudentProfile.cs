using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class StudentProfile : Profile
  {
    public StudentProfile()
    {
      CreateMap<Student, StudentDto>().ReverseMap();
    }
  }
}