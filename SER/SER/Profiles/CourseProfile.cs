using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class CourseProfile : Profile
  {
    public CourseProfile()
    {
      CreateMap<Course, CourseDto>().ReverseMap();
    }
  }
}