using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SER.Pages.Menus;

[Authorize(Roles = "Coordinador")]
public class UICoordinador : PageModel
{
    [HttpPost]
    public void OnPostCerrarSesion()
    {
        Console.WriteLine("Cerrar sesion");
    }
}