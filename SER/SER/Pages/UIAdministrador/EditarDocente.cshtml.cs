using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;

public class EditarDocent : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty] public Profesor profesorNuevo { get; set; }
    [BindProperty] public string nombre { get; set; }
    [BindProperty] public string apellidop { get; set; }
    [BindProperty] public string apellidom { get; set; }

    public EditarDocent(MySERContext context)
    {
        _context = context;
        profesorNuevo = new Profesor();
        nombre = "";
        apellidom = "";
        apellidop = "";
    }
    public void OnGet()
    {
        cargarProfesor();
    }

    public void cargarProfesor()
    {
        var profesor = _context.Profesors.FirstOrDefault(p => p.ProfesorId == Int32.Parse(Request.Query["id"]));
        string[] nombreCompleto = profesor.Nombre.Split(' ');
        nombre = nombreCompleto[0];
        apellidop = nombreCompleto[1];
        apellidom = nombreCompleto[2];
        profesorNuevo.NumeroDePersonal = profesor.NumeroDePersonal;
    }

    public void OnPost()
    {
        try
        {
            var profesor = _context.Profesors.FirstOrDefault(p => p.ProfesorId == Int32.Parse(Request.Query["id"]));
            profesor.Nombre = Request.Form["nombreProfesor"] + " " + Request.Form["apellidoPaterno"] + " " +
                              Request.Form["apellidoMaterno"];
            profesor.Nombre = profesor.Nombre.ToUpper();
            profesor.NumeroDePersonal = Request.Form["numeroPersonal"];
            _context.SaveChanges();
            TempData["Success"] = "Profesor actualizado correctamente";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al actualizar el profesor, " + e.Message;
        }
    }
}