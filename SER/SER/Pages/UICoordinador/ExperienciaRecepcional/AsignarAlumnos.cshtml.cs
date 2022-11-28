using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class AsignarAlumnos : PageModel
{
    private readonly MySERContext _context;
    public List<Alumno> Alumnos { get; set; }
    
    public AsignarAlumnos(MySERContext context)
    {
        _context = context;
        Alumnos = new List<Alumno>();
    }
    
    public IActionResult OnPostGuardarRegistroEstudiantes(string nombreEstudiante, string matriculaEstudiante, string trabajoId, string justificacionIntegrantes)
    {
        try
        {
            AlumnoTrabajoRecepcional alumnoTrabajoRecepcional = new AlumnoTrabajoRecepcional()
            {
                Nombre = nombreEstudiante,
                AlumnoId = matriculaEstudiante,
                TrabajoRecepcionalId = Int32.Parse(trabajoId)
            };
            var trabajo =
                _context.TrabajoRecepcionals.FirstOrDefault(t => t.TrabajoRecepcionalId == Int32.Parse(trabajoId));
            trabajo.JustificacionAlumnos = justificacionIntegrantes;
            _context.AlumnoTrabajoRecepcionals.Add(alumnoTrabajoRecepcional);
            _context.SaveChanges();
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            return new JsonResult(new { success = false});
        }
    }
    
    public JsonResult OnPostEliminarEstudianteAsignado(string matriculaEstudiante, string trabajoId)
    {
        try
        {
            var estudiante = _context.AlumnoTrabajoRecepcionals.First(a =>
                a.AlumnoId == matriculaEstudiante && a.TrabajoRecepcionalId == Int32.Parse(trabajoId));
            _context.Remove(estudiante);
            _context.SaveChanges();
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return new JsonResult(new { success = false});
        }
    }
    
    public void OnGet()
    {
    }

    public JsonResult OnGetObtenerEstudiantesAsignados(string idProyecto)
    {
        return new JsonResult(_context.AlumnoTrabajoRecepcionals
            .Where(a => a.TrabajoRecepcionalId == Int32.Parse(idProyecto)).ToList().ToJson());
    }
    
    public JsonResult OnGetObtenerEstudiantesDisponibles()
    {
        try
        {
            var listaAsignados = _context.AlumnoTrabajoRecepcionals.ToList();
            var listaAlumnos = _context.Alumnos.ToList();

            foreach (var alumno in listaAlumnos)
            {
                if (!listaAsignados.Any(a => a.AlumnoId == alumno.Matricula))
                {
                    Alumno alumnoDisponible = new Alumno()
                    {
                        Matricula = alumno.Matricula,
                        Nombre = alumno.Nombre,
                    };
                    Alumnos.Add(alumnoDisponible);
                }
            }
            return new JsonResult(Alumnos.ToJson());
        }
        catch (Exception e)
        {
            return new JsonResult(new {Error = "Ha ocurrido un error al cargar la información, "+e.Message});
        }
    }
}