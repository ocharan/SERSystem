using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class Organizaciones : PageModel
{
    private readonly MySERContext _context;
    
    [BindProperty]
    public Organizacion Organizacion { get; set; }
    public List<Organizacion> OrganizacionesList { get; set;}

    public Organizaciones(MySERContext context)
    {
        _context = context;
        OrganizacionesList = new List<Organizacion>();
    }

    public JsonResult OnPostGuardarOrganizacion(string NombreOrg)
    {
        Organizacion.Nombre = NombreOrg;
        var result = 0;
        try
        {
            var listaOrganizaciones = _context.Organizacions.ToList();
            bool existeOrg = listaOrganizaciones.Any(o => o.Nombre == Organizacion.Nombre);
            if (!existeOrg)
            {
                result = 1;
                _context.Organizacions.Add(Organizacion);
                _context.SaveChanges();
                return new JsonResult(result);
            }
            else
            {
                result = 2;
                return new JsonResult(result);
            }
        }
        catch (Exception e)
        {
            result = -1;
            return new JsonResult(result);
        }
    }

    public void OnGet()
    {
        CargarOrganizaciones();
    }

    public void CargarOrganizaciones()
    {
        var listaOrganizaciones = _context.Organizacions.ToList();
        foreach (var organizacion in listaOrganizaciones)
        {
            Organizacion org = new Organizacion()
            {
                Nombre = organizacion.Nombre,
                OrganizacionId = organizacion.OrganizacionId
            };
            OrganizacionesList.Add(org);
        }
    }
}