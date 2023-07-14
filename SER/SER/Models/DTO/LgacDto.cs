using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class LgacDto
  {
    [Key]
    public int LgacId { get; set; }
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(100, ErrorMessage = "El nombre debe tener entre {2} y {1} carácteres.", MinimumLength = 5)]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]
    [StringLength(1000, ErrorMessage = "La descripción debe tener entre {2} y {1} carácteres.", MinimumLength = 5)]
    public string Description { get; set; } = null!;
    public int AcademicBodyId { get; set; }
  }
}