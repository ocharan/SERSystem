using SER.Models.DTO;

namespace SER.Services
{
  public interface ICourseService
  {
    (IQueryable<CourseDto> Courses, int OpenCount, int ClosedCount) GetAllCourses(string filter);
    Task<CourseDto> GetCourse(int courseId);
    Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId);
    Task<Dictionary<string, bool>> CreateCourse(CourseDto courseDto);
    Task<Dictionary<string, bool>> UpdateCourse(CourseDto courseDto);
  }
}