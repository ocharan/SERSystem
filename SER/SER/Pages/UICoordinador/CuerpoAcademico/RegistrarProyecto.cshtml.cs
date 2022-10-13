using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistrarProyecto : PageModel
{

    [BindProperty]
    public string tipoRegistro { get; set; }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        try
        {
            if (tipoRegistro.Equals("Proyecto de investigación"))
            {
                return RedirectToPage("RegistroProyectoDeInvestigacion");
            }else if (tipoRegistro.Equals("pladea"))
            {
                return RedirectToPage("RegistroPLADEA");
            }
            else
            {
                return RedirectToPage("RegistroVinculacion");
            }
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "Debe seleccionar un tipo de proyecto";
            return Page();
        }
    }
}