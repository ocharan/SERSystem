using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class EditarTipoDocumento : PageModel
{
    public void OnGet()
    {
        
    }
}