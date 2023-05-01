using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class CourseFileProfile : Profile
  {
    public CourseFileProfile()
    {
      CreateMap<CourseFile, CourseFileDto>().ReverseMap();
    }
  }
}