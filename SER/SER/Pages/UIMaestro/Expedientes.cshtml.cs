using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIMaestro;

public class Expedientes : PageModel
{

    private readonly MySERContext _context;
    
    public List<Expediente> listaExpedientes { get; set; }

    public Expedientes(MySERContext context)
    {
        _context = context;
        listaExpedientes = new List<Expediente>();
    }
    public void OnGet()
    {
        try
        {
            cargarExpedientes();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IActionResult OnPostRegistrarDocumento()
    {
        var trabajo =
            _context.TrabajoRecepcionals.FirstOrDefault(t =>
                t.TrabajoRecepcionalId.Equals(Int32.Parse(Request.Query["id"])));
        if (trabajo.ExperienciaActual.Equals("er"))
        {
            return Redirect("RegistrarDocumentoExperiencia?id="+trabajo.TrabajoRecepcionalId);
        }
        else
        {
            return Redirect("RegistrarDocumentoProyectoGuiado?id=" + trabajo.TrabajoRecepcionalId);
        }
    }
    
    public IActionResult OnPostExpediente()
    {
        return Redirect("VerExpediente?id=" + Request.Query["id"]);
    }

    public void cargarExpedientes()
    {
        var listaAlumnos = new List<AlumnoExperienciaEducativa>();
        var alumnos = _context.AlumnoExperienciaEducativas.ToList();
        var profesor = _context.Profesors.FirstOrDefault(p => p.NumeroDePersonal.Equals(Request.Query["id"]));
        var experiencias = _context.ExperienciaEducativas.Where(e => e.ProfesorId == profesor.ProfesorId)
            .ToList();
        foreach (var alumno in alumnos)
        {
            foreach (var exp in experiencias)
            {
                if (alumno.ExperienciaEducativaId == exp.ExperienciaEducativaId)
                {
                    listaAlumnos.Add(alumno);
                }
            }
        }

        var expedientes = _context.Expedientes.ToList();
        foreach (var alumno in listaAlumnos)
        {
            foreach (var exp in expedientes)
            {
                if (exp.Matricula.Equals(alumno.AlumnoId))
                {
                    Expediente expediente = new Expediente()
                    {
                        Nombre = exp.Nombre,
                        NombreAlumno = alumno.Nombre,
                        Matricula = exp.Matricula,
                        TrabajoRecepcionalId = exp.TrabajoRecepcionalId,
                        Estado = exp.Estado,
                        Modalidad = exp.Modalidad
                    };
                    listaExpedientes.Add(expediente);
                }
            }
        }
    }
}