using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SER.Pages.Menus;

[Authorize(Roles = "Administrador")]
public class UIAdministración : PageModel
{
    public void OnGet()
    {
        
    }
}