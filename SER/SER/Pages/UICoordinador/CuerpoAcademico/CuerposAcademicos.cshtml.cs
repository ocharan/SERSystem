using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class CuerposAcademicos : PageModel
{

    private readonly MySERContext _context;

    [BindProperty] public string idAcademico { get; set; }
    public List<Entidades.CuerpoAcademico> CuerpoAcademicos { get; set; }
    
    public CuerposAcademicos(MySERContext context)
    {
        _context = context;
        CuerpoAcademicos = new List<Entidades.CuerpoAcademico>();

    }

    public void OnGet()
    {
        getCuerposAcademicos();
    }


    [HttpPost]
    public IActionResult OnPost()
    {
        var id = Request.Query["id"];
        return Redirect("RegistrarIntegrantesCA?id="+id);
    }
    public void getCuerposAcademicos()
    {
        try
        {
            var listaCuerpos = _context.CuerpoAcademicos.ToList();
            foreach (var cuerpoCA in listaCuerpos)
            {
                Entidades.CuerpoAcademico cuerpo = new Entidades.CuerpoAcademico()
                {
                    Nombre = cuerpoCA.Nombre,
                    CuerpoAcademicoId = cuerpoCA.CuerpoAcademicoId,
                };
                CuerpoAcademicos.Add(cuerpo);
            }
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = "Error al cargar los cuerpos academicos";
        }
    }
    
  
}