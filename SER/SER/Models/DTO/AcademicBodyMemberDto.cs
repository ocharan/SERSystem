namespace SER.Models.DTO
{
  public class AcademicBodyMemberDto
  {
    public int AcademicBodyMemberId { get; set; }
    public int AcademicBodyId { get; set; }
    public int ProfessorId { get; set; }
    public string Role { get; set; } = null!;
    public AcademicBodyDto AcademicBody { get; set; } = null!;
    public ProfessorDto Professor { get; set; } = null!;
  }
}