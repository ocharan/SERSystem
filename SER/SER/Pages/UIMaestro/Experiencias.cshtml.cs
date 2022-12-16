using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.DTO;

namespace SER.Pages.UIMaestro;

[Authorize(Roles = "Maestro")]
public class Experiencias : PageModel
{

    private readonly MySERContext Context;
    
    public List<ExperienciaAlumno> experienciasAdscritas { get; set; }

    public Experiencias(MySERContext _context)
    {
        Context = _context;
        experienciasAdscritas = new List<ExperienciaAlumno>();
    }
    
    public void OnGet()
    {
        try
        {
            cargarExperiencias();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información solicitada";
        }
    }

    public IActionResult OnPostVerAlumnos()
    {
        return Redirect("Alumnos?experiencia=" + Request.Query["experiencia"]+"&&?id="+Request.Query["id"]);
    }

    public void cargarExperiencias()
    {
        var profesor = Context.Profesors.FirstOrDefault(p => p.NumeroDePersonal.Equals(Request.Query["id"]));
        var listaExperiencias = Context.ExperienciaEducativas.Where(e => e.ProfesorId == profesor.ProfesorId).ToList();
        var listaAlumnos = Context.Alumnos.ToList();
        var listaAdscritos = Context.AlumnoExperienciaEducativas.ToList();
        foreach (var exp in listaExperiencias)
        {
            int totalAlumnos = 0;
            foreach (var alumno in listaAlumnos)
            {
                if (listaAdscritos.Any(a =>
                        a.ExperienciaEducativaId.Equals(exp.ExperienciaEducativaId) &&
                        alumno.Matricula.Equals(a.AlumnoId)))
                {
                    totalAlumnos++;
                }
            }

            ExperienciaAlumno experienciaAlumno = new ExperienciaAlumno()
            {
                nombreExperiencia = exp.Nombre,
                NRC = exp.Nrc,
                periodo = exp.Periodo,
                experiendiaID = exp.ExperienciaEducativaId,
                cantidadAlumnos = totalAlumnos
            };
            experienciasAdscritas.Add(experienciaAlumno);
        }
    }
}