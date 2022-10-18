using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
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

    public JsonResult OnGetBuscarAlumno(string queryAlumno)
    {
        try
        {
            IQueryable<Alumno> resultadoBusqueda;
            if (queryAlumno == null )
            {
                resultadoBusqueda = _context.Alumnos;
            }
            else
            {
                resultadoBusqueda = _context.Alumnos.Where(a => a.Nombre.Contains(queryAlumno));
            }
            if (resultadoBusqueda.Count() > 0)
            {
                AlumnosLista.Clear();
                foreach (var resultado in resultadoBusqueda)
                {
                    Alumno alumnoRegistrado = new Alumno()
                    {
                        Nombre = resultado.Nombre,
                        Matricula = resultado.Matricula,
                        CorreoElectronico = resultado.CorreoElectronico
                    };
                    AlumnosLista.Add(alumnoRegistrado);
                }
                return new JsonResult(AlumnosLista.ToJson());
            }
            else
            {
                return new JsonResult("1".ToJson());
            }
        }
        catch (Exception e)
        {
            return new JsonResult("0".ToJson());
        }
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