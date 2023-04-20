using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SER.Models.DTO;
using SER.Configuration;
using SER.Services;
using AutoMapper;

namespace SER.Pages
{
  public class IndexModel : PageModel
  {
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private const string ERROR_MESSAGE = "Usuario y/o contraseña no válidos.";
    [BindProperty]
    public UserDto user { set; get; } = null!;

    public IndexModel(IUserService userService, IMapper mapper)
    {
      _userService = userService;
      _mapper = mapper;
    }

    public IActionResult OnGet() { return Page(); }

    public async Task SignInUser(UserDto user)
    {
      try
      {
        List<Claim> claims = new List<Claim> {
          new Claim(ClaimTypes.Name, user.Username!),
          new Claim(ClaimTypes.Role, user.Role!),
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(
          claims, CookieAuthenticationDefaults.AuthenticationScheme
        );

        await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity)
        );
      }
      catch (Exception)
      {
        throw new Exception("Error al tratar de conectarse con el servidor");
      }
    }

    public async Task<IActionResult> OnPostLogOut()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return RedirectToPage("/Index");
    }

    public RedirectToPageResult GetPageResult(string userRole, string username)
    {
      Dictionary<string, (string Page, object RouteValues)> routes =
        new Dictionary<string, (string Page, object RouteValues)> {
          {"Coordinador", ("/Menus/UICoordinador", "")},
          {"Administrador", ("/Menus/Management", "")},
          {"Maestro", ("/Menus/UIMaestro", new { id = username })},
        };

      return RedirectToPage(
        routes[userRole].Page,
        routes[userRole].RouteValues
      );
    }

    public async Task<IActionResult> OnPostLogin()
    {
      try
      {
        UserDto obtainedUser = await _userService.GetUserByUsername(user.Username);
        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(user.Password, obtainedUser.Password);

        if (isPasswordCorrect)
        {
          await SignInUser(obtainedUser);

          return GetPageResult(obtainedUser.Role, obtainedUser.Username);
        }
        else
        {
          ViewData["MessageError"] = ERROR_MESSAGE;
        }
      }
      catch (Exception ex)
      {
        ViewData["MessageError"] = ex.Message.Equals("Usuario no encontrado")
          ? ERROR_MESSAGE : ex.Message;

        ExceptionLogger.LogException(ex);
      }

      return Page();
    }
  }
}