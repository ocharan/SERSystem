
using AutoMapper;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Profiles
{
  public class CourseRegistrationProfile : Profile
  {
    public CourseRegistrationProfile()
    {
      CreateMap<CourseRegistration, CourseRegistrationDto>().ReverseMap();
    }
  }
}