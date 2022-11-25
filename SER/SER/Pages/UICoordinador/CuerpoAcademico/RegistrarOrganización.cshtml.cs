using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistrarOrganización : PageModel
{
    private readonly MySERContext _context;
    
    [BindProperty]
    public Organizacion Organizacion { get; set; }

    public RegistrarOrganización(MySERContext context)
    {
        Organizacion = new Organizacion();
        _context = context;
    }
    
    public void OnGet()
    {
        
    }
    
    public void OnPost()
    {
        Console.WriteLine(Organizacion.Nombre);
        try
        {
            var listaOrganizaciones = _context.Organizacions.ToList();
            bool existeOrg = listaOrganizaciones.Any(o => o.Nombre == Organizacion.Nombre);
            if (!existeOrg)
            {
                _context.Organizacions.Add(Organizacion);
                _context.SaveChanges();
                TempData["Success"] = "Organización registrada correctamente";
            }
            else
            {
                TempData["Error"] = "La organización que intentas registrar con ese nombre, ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error" + e.Message;
        }
    }
}