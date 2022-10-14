using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages
{
    public class RegistrarSinodalModel : PageModel
    {
        [BindProperty] public string tipoRegistro { get; set; }

        public void OnGet()
        {
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