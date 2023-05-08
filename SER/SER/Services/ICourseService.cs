using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface ICourseService
  {
    IQueryable<CourseDto> GetAllCourses();
    Task<CourseDto> GetCourse(int courseId);
    Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId);
    Task<Response> CreateCourse(CourseDto courseDto, IFormFile? file = null);
    Task<Response> CreateCourseRegistrations(List<CourseRegistrationDto> registrations);
    Task<Response> CreateProfessorAssignment(int courseId, int professorId);
  }
}