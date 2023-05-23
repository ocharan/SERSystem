using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class UserDto
  {
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Campo requerido.")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido.")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [StringLength(64, ErrorMessage = "El correo electrónico debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido.")]
    public string Role { get; set; } = null!;
    public string? RecoveryToken { get; set; }
  }
}
