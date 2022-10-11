using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistrarCuerpoAcademico : PageModel
{
    private readonly MySERContext _context;
    
    [BindProperty] 
    public Entidades.CuerpoAcademico _cuerpoAcademico { get; set; }
    
    public List<Integrante> Integrantes { get; set; }
    public List<Profesor> Profesors { get; set; }

    public RegistrarCuerpoAcademico(MySERContext context)
    {
        _context = context;
        Profesors = new List<Profesor>();
        Integrantes = new List<Integrante>();
    }
    
    public void OnGet()
    {
    }

    public void OnPost()
    {
        try
        {
            var cuerposExistentes = _context.CuerpoAcademicos.ToList();
            bool existeCuerpo = cuerposExistentes.Any(c => c.Nombre.Equals(_cuerpoAcademico.Nombre));
            if (!existeCuerpo)
            {
                _context.CuerpoAcademicos.Add(_cuerpoAcademico);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Cuerpo academico registrado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "El cuerpo academico que intentas registrar ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = e.Data;
        }
    }

   

}