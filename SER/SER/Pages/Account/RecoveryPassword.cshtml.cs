using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models;
using SER.Configuration;
using System.ComponentModel.DataAnnotations;
using SER.Models.DTO;

namespace SER.Pages.Account
{
  public class RecoveryPasswordModel : PageModel
  {
    public bool IsMailSent { set; get; }
    public bool IsValidForm { set; get; }
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    [BindProperty]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string username { set; get; } = null!;
    private readonly IConfiguration Configuration;

    public RecoveryPasswordModel(IUserService userService, IEmailService emailService, ITokenService tokenService, IConfiguration configuration)
    {
      _userService = userService;
      _emailService = emailService;
      _tokenService = tokenService;
      Configuration = configuration;
    }

    public IActionResult OnGet() { return Page(); }

    public EmailMessage CreateEmailMessage(string userEmail, string token)
    {
      string clientUrl = Configuration.GetSection("ClientUrl").Value;
      string recoveryLink = $"{clientUrl}/Account/ChangePassword?token={token}";

      EmailMessage emailMessage = new EmailMessage(
        to: userEmail,
        subject: "Recuperación de contraseña",
        content:
        "Para recuperar su contraseña, haga clic en el siguiente enlace: " +
         $"<a href='{recoveryLink}'>Recuperar contraseña</a>"
      );

      return emailMessage;
    }

    public async Task<IActionResult> OnPostAssignToken()
    {
      try
      {
        UserDto obtainedUser = await _userService.GetUserByUsername(username);
        string tokenRecovery = await _userService.GenerateRecoveryToken(obtainedUser.Email);
        EmailMessage emailMessage = CreateEmailMessage(obtainedUser.Email, tokenRecovery);
        IsMailSent = await _emailService.SendEmailAsync(emailMessage);
        ViewData["MessageSuccess"] = "Se ha enviado un mensaje a su correo electrónico con el enlace para acceder y reestablecer su contraseña.";
      }
      catch (Exception ex)
      {
        ViewData["MessageError"] = ex.Message;
        ExceptionLogger.LogException(ex);
      }

      return Page();
    }
  }
}