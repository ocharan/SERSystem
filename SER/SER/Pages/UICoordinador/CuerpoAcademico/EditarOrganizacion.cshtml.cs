using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarOrganizacion : PageModel
{
    
    private readonly MySERContext _context;
    
    [BindProperty]
    public Organizacion Organizacion { get; set; }
    
    public EditarOrganizacion(MySERContext context)
    {
        _context = context;
        Organizacion = new Organizacion();
    }
    public void OnGet()
    {
        try
        {
            cargarOrganizacion();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Error al cargar la información";
        }
    }

    public void cargarOrganizacion()
    {
        var idOrg = Int32.Parse(Request.Query["id"]);
        var orgObtenida = _context.Organizacions.FirstOrDefault(o => o.OrganizacionId == idOrg);
        Organizacion.Nombre = orgObtenida.Nombre;
    }

    public void OnPost()
    {
        try
        {
            var idOrg = Int32.Parse(Request.Query["id"]);
            var existe = _context.Organizacions.Any(o => o.Nombre == Organizacion.Nombre);
            if (!existe)
            {
                var org = _context.Organizacions.First(o => o.OrganizacionId == idOrg);
                org.Nombre = Organizacion.Nombre;
                _context.SaveChanges();
                TempData["Success"] = "Orgnización actualizada correctamente";
            }
            else
            {
                TempData["Error"] = "El nombre que intenta ingresar ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Error al tratar de guardar los cambios"; 
        }
    }
}