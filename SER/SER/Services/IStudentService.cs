using SER.Models.DTO;

namespace SER.Services
{
  public interface IStudentService
  {
    (IQueryable<StudentDto> Students, int AssignedCount, int UnassignedCount) GetAllStudents(string filter);
    Task<StudentDto> GetStudent(int studentId);
    Task<Dictionary<string, bool>> CreateStudent(StudentDto studentDto);
    Task<Dictionary<string, bool>> UpdateStudent(StudentDto studentDto);
  }
}