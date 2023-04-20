using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SER.Models.DB;

namespace SER.Pages.Menus
{
  [Authorize(Roles = "Administrador")]
  public class ManagementModel : PageModel
  {
    private readonly SERContext _context;

    public ManagementModel(SERContext context)
    {
      _context = context;
    }

    public IActionResult OnGet() { return Page(); }
  }
}
