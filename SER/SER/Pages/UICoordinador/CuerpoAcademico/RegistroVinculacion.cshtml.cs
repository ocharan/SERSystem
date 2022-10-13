using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistroVinculacion : PageModel
{

    private readonly MySERContext _context;

    public Vinculacion Vinculacion { get; set; }
    public List<Organizacion> OrganizacionesList { get; set; }

    public RegistroVinculacion(MySERContext context)
    {
        _context = context;
        OrganizacionesList = new List<Organizacion>();
        Vinculacion = new Vinculacion();
    }
    
    public void OnGet()
    {
        cargarOrganizaciones();
    }

    public void OnPost()
    {
        try
        {
            Vinculacion.FechaDeInicioDeConvenio = DateTime.Parse(Request.Form["FechaInicio"]);
            Vinculacion.OrganizacionIid = Convert.ToInt32(Request.Form["orgId"]);
            bool existe = _context.Vinculacions.ToList().Any(v => v.OrganizacionIid.Equals(Vinculacion.OrganizacionIid));
            if (!existe)
            {
                _context.Vinculacions.Add(Vinculacion);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Registro completado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "El proyecto vinculación ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Error al registrar el proyecto vinculación " + e.Message;
        }
    }
    
    public void cargarOrganizaciones()
    {
        var listaOrg = _context.Organizacions.ToList();
        foreach (var org in listaOrg)
        {
            Organizacion orgExistente = new Organizacion()
            {
                Nombre = org.Nombre,
                OrganizacionId = org.OrganizacionId
            };
            OrganizacionesList.Add(orgExistente);
        }
    }
}