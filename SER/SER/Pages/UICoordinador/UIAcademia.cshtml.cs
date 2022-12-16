using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SER.Pages.UICoordinador;

[Authorize(Roles = "Coordinador")]
public class UIAcademia : PageModel
{
    public void OnGet()
    {
        
    }
}