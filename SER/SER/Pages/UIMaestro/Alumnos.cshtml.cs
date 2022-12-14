using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIMaestro;

public class Alumnos : PageModel
{

    private readonly MySERContext _context;
    
    public List<AlumnoExperienciaEducativa> alumnosAdscritos { get; set; }

    public Alumnos(MySERContext context)
    {
        _context = context;
        alumnosAdscritos = new List<AlumnoExperienciaEducativa>();
    }
    public void OnGet()
    {
        try
        {
            cargarAlumnos();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información solicitada";
        }
    }

    public void cargarAlumnos()
    {
        var alumnos =
            _context.AlumnoExperienciaEducativas.Where(
                a => a.ExperienciaEducativaId == Int32.Parse(Request.Query["experiencia"]));
        foreach (var alumno in alumnos)
        {
            AlumnoExperienciaEducativa alumnoExp = new AlumnoExperienciaEducativa()
            {
                AlumnoId = alumno.AlumnoId,
                Nombre = alumno.Nombre
            };
            alumnosAdscritos.Add(alumnoExp);
        }
    }
}