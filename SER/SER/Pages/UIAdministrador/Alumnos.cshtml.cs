using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UIAdministrador;

public class Alumnos : PageModel
{
    private readonly MySERContext _context;

    public List<Alumno> AlumnosLista { get; set; }

    public Alumnos(MySERContext context)
    {
        _context = context;
        AlumnosLista = new List<Alumno>();
    }
    
    
    public void OnGet()
    {
        cargarAlumnos();
    }

    public void cargarAlumnos()
    {
        var listaAlumnos = _context.Alumnos.ToList();
        foreach (var alumno in listaAlumnos)
        {
            Alumno alumnoRegistrado = new Alumno()
            {
                Nombre = alumno.Nombre,
                Matricula = alumno.Matricula,
                CorreoElectronico = alumno.CorreoElectronico
            };
            AlumnosLista.Add(alumnoRegistrado);
        }
    }
}