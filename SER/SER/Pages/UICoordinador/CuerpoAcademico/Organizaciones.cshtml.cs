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