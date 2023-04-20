using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;

namespace SER.Pages.Account
{
  public class ChangePasswordModel : PageModel
  {
    public bool IsValidToken { set; get; }
    public bool IsChangePassword { set; get; }
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    [BindProperty]
    public LoginDto user { set; get; } = null!;

    public ChangePasswordModel(IUserService userService, ITokenService tokenService)
    {
      _userService = userService;
      _tokenService = tokenService;
    }

    public async Task OnGet()
    {
      string token = Request.Query["token"];
      IsValidToken = await _userService.IsValidUserToken(token);
    }

    public async Task<IActionResult> OnPostChangePassword()
    {
      IsValidToken = await _userService.IsValidUserToken(Request.Form["token"]);

      if (!IsValidToken || !ModelState.IsValid) { return Page(); }

      try
      {
        string token = Request.Form["token"];
        IsChangePassword = await _userService.UpdateUserPassword(token, user.NewPassword!);
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
      }

      return Page();
    }
  }
}
