using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface ICourseService
  {
    Task<bool> IsCourseExisting(string nrc);
    IQueryable<CourseDto> GetAllCourses();
    Task<CourseDto> GetCourse(int courseId);
    Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId);
    Task<Response> CreateCourse(CourseDto courseDto, IFormFile? file = null);
  }
}