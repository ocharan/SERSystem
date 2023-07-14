using SER.Models.DTO;
using SER.Models.Responses;
using SER.Models.DB;

namespace SER.Services
{
  public interface ICourseService
  {
    IQueryable<CourseDto> GetAllCourses();
    IQueryable<CourseDto> GetProfessorCourses(string userId);
    Task<CourseDto> GetCourse(int courseId);
    Task<CourseFileDto> GetCourseFile(int fileId);
    Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId);
    Task<Response> CreateCourse(CourseDto courseDto, IFormFile? file = null);
    Task<Response> CreateCourseRegistrations(List<CourseRegistrationDto> registrations);
    Task<Response> CreateProfessorAssignment(int courseId, int professorId);
    Task<Response> DeleteCourseRegistration(int registrationId);
    Task<Response> UpdateCourse(CourseDto courseDto, IFormFile? file = null);
    Task<Response> DeleteCourseFile(int fileId);
  }
}