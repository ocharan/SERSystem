using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class EditarAlumno : PageModel
{
    private readonly MySERContext _context;
    
    public string idAlumno { get; set; }
    [BindProperty] public Alumno Alumno { get; set; }
    
    [BindProperty] public string nombre { get; set; }
    [BindProperty] public string apellidoPaterno { get; set; }
    [BindProperty] public string apellidoMaterno { get; set; }

    public EditarAlumno(MySERContext context)
    {
        _context = context;
        Alumno = new Alumno();
        nombre = "";
        apellidoMaterno = "";
        apellidoPaterno = "";
    }
    public void OnGet()
    {
        try
        {
            getAlumno();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void OnPost()
    {
        try
        {
            idAlumno = Request.Query["id"];
            var alumno = _context.Alumnos.FirstOrDefault(a => a.Matricula == idAlumno);
            alumno.Nombre = nombre + " " + apellidoPaterno + " " + apellidoMaterno;
            alumno.Nombre = alumno.Nombre.ToUpper();
            alumno.CorreoElectronico = Request.Form["correo"];
            _context.SaveChanges();
            TempData["Success"] = "Alumno actualizado correctamente";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al actualizar el alumno"+ e.InnerException;
        }
    }

    public void getAlumno()
    {
        idAlumno = Request.Query["id"];
        var alumno = _context.Alumnos.FirstOrDefault(a => a.Matricula == idAlumno);
        string[] nombreCompleto = alumno.Nombre.Split(' ');
        nombre = nombreCompleto[0];
        apellidoPaterno = nombreCompleto[1];
        apellidoMaterno = nombreCompleto[2];
        Alumno.Matricula = alumno.Matricula;
        Alumno.CorreoElectronico = alumno.CorreoElectronico;
    }
}