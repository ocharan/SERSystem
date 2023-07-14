using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class LoginDto
  {
    [Required(ErrorMessage = "Campo requerido.")]
    [StringLength(64, ErrorMessage = "La contraseña debe tener entre {2} y {1} carácteres.", MinimumLength = 10)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una minuscula y un número.")]
    public string? NewPassword { get; set; }
    [Required(ErrorMessage = "Campo requerido.")]
    [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
    public string? ConfirmPassword { get; set; }
  }
}