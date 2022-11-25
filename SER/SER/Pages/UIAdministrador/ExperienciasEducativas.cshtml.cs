using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;

public class ExperienciasEducativas : PageModel
{

    private readonly MySERContext _context;
    
    public List<ExperienciaEducativa> ExperienciaEducativas { get; set; }


    public ExperienciasEducativas(MySERContext context)
    {
        _context = context;
        ExperienciaEducativas = new List<ExperienciaEducativa>();
    }
    
    public void OnGet()
    {
        cargarExperiencias();
    }

    public IActionResult OnPostAsignar()
    {
        return Redirect("AsignarAlumnos?id=" + Request.Query["id"]);
    }

    public IActionResult OnPostEditar()
    {
        return Redirect("EditarExperienciaEducativa?id=" + Request.Query["id"]);
    }
    
    public void cargarExperiencias()
    {
        var experiencias = _context.ExperienciaEducativas.ToList();
        foreach (var experiencia in experiencias)
        {
            ExperienciaEducativa exp = new ExperienciaEducativa()
            {
                Nombre = experiencia.Nombre,
                Nrc = experiencia.Nrc,
                Periodo = experiencia.Periodo,
                EstadoAbierto = experiencia.EstadoAbierto,
                ExperienciaEducativaId = experiencia.ExperienciaEducativaId
            };
            ExperienciaEducativas.Add(exp);
        }
    }
}