using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class ProfessorDto
  {
    public int ProfessorId { get; set; }
    public string FullName { get; set; } = null!;
    public string AcademicDegree { get; set; } = null!;
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
  }
}