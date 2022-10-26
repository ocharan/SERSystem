using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UIAdministrador;

public class EditarExperienciaEducativa : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty] public ExperienciaEducativa ExperienciaEducativa { get; set; }
    [BindProperty] public bool estado { get; set; }
    public EditarExperienciaEducativa(MySERContext context)
    {
        _context = context;
        ExperienciaEducativa = new ExperienciaEducativa();
        estado = true;
    }
    public void OnGet()
    {
        cargarExperiencia();
    }

    public void cargarExperiencia()
    {
        var experiencia =
            _context.ExperienciaEducativas.First(e => e.ExperienciaEducativaId == Int32.Parse(Request.Query["id"]));
        ExperienciaEducativa.Nombre = experiencia.Nombre;
        ExperienciaEducativa.Nrc = experiencia.Nrc;
        ExperienciaEducativa.Periodo = experiencia.Periodo;
        ExperienciaEducativa.Seccion = experiencia.Seccion;
        if (experiencia.EstadoAbierto == 1)
        {
            estado = false;
        }
        else
        {
            estado = true;
        }
    }
}