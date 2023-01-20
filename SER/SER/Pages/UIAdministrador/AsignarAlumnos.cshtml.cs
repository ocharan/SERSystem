using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class AsignarAlumnos : PageModel
{
    private readonly MySERContext _context;

    public List<Alumno> Alumnos { get; set; }
    public AsignarAlumnos(MySERContext context)
    {
        _context = context;
        Alumnos = new List<Alumno>();
    }
    
    public JsonResult OnGetObtenerEstudiantesEEDisponibles()
    {
        try
        {
            var listaAsignados = _context.AlumnoExperienciaEducativas.ToList();
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
    
    public JsonResult OnGetObtenerEstudiantesAsignadosEE(string experienciaId)
    {
        return new JsonResult(_context.AlumnoExperienciaEducativas
            .Where(a => a.ExperienciaEducativaId == Int32.Parse(experienciaId)).ToList().ToJson());
    }
    
    public void OnGet()
    {
        
    }
    
    public IActionResult OnPostGuardarRegistroEstudiantesEE(string nombreEstudiante, string matriculaEstudiante, string experienciaId)
    {
        try
        {
            var expNombre = _context.ExperienciaEducativas
                .FirstOrDefault(e => e.ExperienciaEducativaId == Int32.Parse(experienciaId)).Nombre;
            AlumnoExperienciaEducativa alumnoExperienciaEducativa = new AlumnoExperienciaEducativa()
            {
                Nombre = nombreEstudiante,
                AlumnoId = matriculaEstudiante,
                ExperienciaEducativaId = Int32.Parse(experienciaId),
                NombreExp = expNombre
            };
            _context.AlumnoExperienciaEducativas.Add(alumnoExperienciaEducativa);
            _context.SaveChanges();
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            return new JsonResult(new { success = false});
        }
    }
    
    public JsonResult OnPostEliminarEstudianteAsignadoEE(string matriculaEstudiante, string experienciaId)
    {
        try
        {
            var estudiante = _context.AlumnoExperienciaEducativas.First(a =>
                a.AlumnoId == matriculaEstudiante && a.ExperienciaEducativaId == Int32.Parse(experienciaId));
            _context.AlumnoExperienciaEducativas.Remove(estudiante);
            _context.SaveChanges();
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return new JsonResult(new { success = false});
        }
    }
}