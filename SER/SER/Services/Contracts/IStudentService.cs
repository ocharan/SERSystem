using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface IStudentService
  {
    IQueryable<StudentDto> GetAllStudents();
    Task<StudentDto> GetStudent(int studentId);
    Task<Response> CreateStudent(StudentDto studentDto);
    Task<Response> UpdateStudent(StudentDto studentDto);
    Task<List<StudentDto>> SearchStudent(string search);
    Task<bool> IsStudentExisting(int studentId);
  }
}