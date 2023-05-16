using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface ICourseService
  {
    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>IQueryable of CourseDto</returns>
    IQueryable<CourseDto> GetAllCourses();
    /// <summary>
    /// Get a course by id
    /// </summary>
    /// <param name="courseId">Course id</param>
    /// <returns>CourseDto</returns>    
    Task<CourseDto> GetCourse(int courseId);
    /// <summary>
    /// Get a course file by id
    /// </summary>
    /// <param name="fileId">File id</param>
    /// <returns>CourseFileDto</returns>    
    Task<CourseFileDto> GetCourseFile(int fileId);
    /// <summary>
    /// Get all students courses by student id
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of CourseRegistrationDto</returns>
    Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId);
    /// <summary>
    /// Create a course and optionally a file
    /// </summary>
    /// <param name="courseDto">CourseDto</param>
    /// <param name="file">File</param>
    /// <returns>Response</returns>
    Task<Response> CreateCourse(CourseDto courseDto, IFormFile? file = null);
    /// <summary>
    /// Create a course registrations list by students id and course id
    /// </summary>
    /// <param name="registrations">List of CourseRegistrationDto</param>
    /// <returns>Response</returns>    
    Task<Response> CreateCourseRegistrations(List<CourseRegistrationDto> registrations);
    Task<Response> CreateProfessorAssignment(int courseId, int professorId);
    Task<Response> WithdrawCourseRegistration(int registrationId);
    Task<Response> UpdateCourse(CourseDto courseDto, IFormFile? file = null);
    Task<Response> DeleteCourseFile(int fileId);
  }
}