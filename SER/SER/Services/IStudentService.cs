using SER.Models.DTO;

namespace SER.Services
{
  public interface IStudentService
  {
    IQueryable<StudentDto> GetAllStudents();
    Task<StudentDto> GetStudent(int studentId);
    Task<Dictionary<string, bool>> CreateStudent(StudentDto studentDto);
    Task<Dictionary<string, bool>> UpdateStudent(StudentDto studentDto);
  }
}