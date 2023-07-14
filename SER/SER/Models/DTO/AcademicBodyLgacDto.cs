namespace SER.Models.DTO
{
  public class AcademicBodyLgacDto
  {
    public int AcademicBodyLgacId { get; set; }
    public int AcademicBodyId { get; set; }
    public int LgacId { get; set; }
    public LgacDto Lgac { get; set; } = null!;
  }
}