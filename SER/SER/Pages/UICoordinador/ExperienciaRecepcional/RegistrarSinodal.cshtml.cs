using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages
{
    
    [Authorize(Roles = "Coordinador")]
    public class RegistrarSinodalModel : PageModel
    {
        [BindProperty] public string tipoRegistro { get; set; }

        public void OnGet()
        {
        }
        
        [HttpPost]
        public async Task<IActionResult> OnPostCerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new JsonResult(new { succes = true });
        }

        public IActionResult OnPost()
        {
            try
            {
                if (tipoRegistro.Equals("Interno"))
                {
                    return RedirectToPage("RegistrarSinodalInterno");
                }else
                {
                    return RedirectToPage("RegistrarSinodalExterno");
                }
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = "Debe seleccionar un tipo de proyecto";
                return Page();
            }
        }
    }
}