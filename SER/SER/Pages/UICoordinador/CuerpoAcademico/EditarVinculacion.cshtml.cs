using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarVinculacion : PageModel
{

    private MySERContext _context;

    public string idVinculacion;
    public List<Organizacion> OrganizacionesList { get; set; }

    [BindProperty] public Vinculacion Vinculacion { get; set; }
    
    public EditarVinculacion(MySERContext context)
    {
        _context = context;
        OrganizacionesList = new List<Organizacion>();
        Vinculacion = new Vinculacion();
    }

    public void OnPost()
    {
        try
        {
            idVinculacion = Request.Query["id"];
            var vinculacion = _context.Vinculacions.FirstOrDefault(v => v.VinculacionId == Int32.Parse(idVinculacion));
            bool existe = _context.Vinculacions.Any(v => v.OrganizacionIid == Int32.Parse(Request.Form["orgId"]));
            if (Vinculacion.OrganizacionIid == vinculacion.OrganizacionIid)
            {
                vinculacion.FechaDeInicioDeConvenio = Vinculacion.FechaDeInicioDeConvenio;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Vinculación actualizada correctamente";
            }
            else
            {
                Console.WriteLine(existe);
                if (!existe)
                {
                    vinculacion.OrganizacionIid = Int32.Parse(Request.Form["orgId"]);
                    vinculacion.FechaDeInicioDeConvenio = DateTime.Parse(Request.Form["fechaInicio"]);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Vinculación actualizada correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "La organización vinculada ya existe";
                }
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] =
                "Ha ocurrido un error durante la actualización del proyecto vinculación " + e.Message;
        }
    }
    
    public void OnGet()
    {
        try
        {
            cargarOrganizaciones();
            cargarVinculacion();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información";
        }
    }

    public void cargarVinculacion()
    {
        idVinculacion = Request.Query["id"];
        var vinculacion = _context.Vinculacions.FirstOrDefault(v => v.VinculacionId == Int32.Parse(idVinculacion));
        Vinculacion.OrganizacionIid = vinculacion.OrganizacionIid;
        Vinculacion.FechaDeInicioDeConvenio = vinculacion.FechaDeInicioDeConvenio;

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