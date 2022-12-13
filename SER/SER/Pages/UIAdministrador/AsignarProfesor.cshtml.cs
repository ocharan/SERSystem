using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;

public class AsignarProfesor : PageModel
{
    private readonly MySERContext _context;
    
    public List<Profesor> Profesors { get; set; }
    
    [BindProperty] public  Profesor ProfesorAsignar { get; set; }

    public AsignarProfesor(MySERContext context)
    {
        _context = context;
        Profesors = new List<Profesor>();
        ProfesorAsignar = new Profesor();
    }

    public void OnPostAsignar()
    {
        try
        {
            var experiencia =
                _context.ExperienciaEducativas.FirstOrDefault(e =>
                    e.ExperienciaEducativaId == Int32.Parse(Request.Query["id"]));
            experiencia.ProfesorId = Int32.Parse(Request.Query["idProfesor"]);
            _context.SaveChanges();
            TempData["Success"] = "Se ha asignado el profesor correctamente";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al guardar los datos, "+e.Message;
        }
        
    }
    
    
    public void OnGet()
    {
        try
        {
            getDocentes();
            getProfesorAsignado();
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información";
        }
    }

    public void getProfesorAsignado()
    {
        var experiencia =
            _context.ExperienciaEducativas.FirstOrDefault(e =>
                e.ExperienciaEducativaId == Int32.Parse(Request.Query["id"]));
        var profesor = _context.Profesors.FirstOrDefault(p => p.ProfesorId == experiencia.ProfesorId);
        if (profesor != null)
        {
            ProfesorAsignar.ProfesorId = profesor.ProfesorId;
            ProfesorAsignar.Nombre = profesor.Nombre;
        }
        else
        {
            ProfesorAsignar.Nombre = "Sin profesor asignado";
        }
    }
    public void getDocentes()
    {
        var listaProfesores = _context.Profesors.ToList();
        foreach (var profesor in listaProfesores)
        {
            Profesor docente = new Profesor()
            {
                ProfesorId = profesor.ProfesorId,
                Nombre = profesor.Nombre,
                NumeroDePersonal = profesor.NumeroDePersonal
            };
            Profesors.Add(docente);
        }
    }
}