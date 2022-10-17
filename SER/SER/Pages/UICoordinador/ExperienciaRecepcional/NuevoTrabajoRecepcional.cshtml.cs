using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class NuevoTrabajoRecepcional : PageModel
{

    private readonly MySERContext _context;
    
    public List<Pladeafei> Pladeafeis { get; set; }
    
    public List<Vinculacion> Vinculacions { get; set; }
    
    public List<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }
    public NuevoTrabajoRecepcional(MySERContext context)
    {
        _context = context;
        Pladeafeis = new List<Pladeafei>();
        Vinculacions = new List<Vinculacion>();
        ProyectoDeInvestigacions = new List<ProyectoDeInvestigacion>();
    }
    
    public void OnGet()
    {
    }

    public JsonResult OnGetObtenerPladea()
    {
        var listaPladea = _context.Pladeafeis.ToList();
        foreach (var pladea in listaPladea)
        {
            Pladeafei pladeafei = new Pladeafei()
            {
                Accion = pladea.Accion,
                PladeafeiId = pladea.PladeafeiId
            };
            Pladeafeis.Add(pladeafei);
        }

        return new JsonResult(Pladeafeis.ToJson());
    }

    public JsonResult OnGetObtenerInvestigacion()
    {
        var listaInvestigacion = _context.ProyectoDeInvestigacions.ToList();
        foreach (var investigacion in listaInvestigacion)
        {
            ProyectoDeInvestigacion proyectoDeInvestigacion = new ProyectoDeInvestigacion()
            {
                Nombre = investigacion.Nombre,
                ProyectoDeInvestigacionId = investigacion.ProyectoDeInvestigacionId
            };
            ProyectoDeInvestigacions.Add(proyectoDeInvestigacion);
        }
        return new JsonResult(ProyectoDeInvestigacions.ToJson());
    }

    public JsonResult OnGetObtenerVinculacion()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaOrgs = _context.Organizacions.ToList();
        var vinculaciones = listaVinculacion.Join(listaOrgs, vinculacion => vinculacion.OrganizacionIid,
            organizacion => organizacion.OrganizacionId, (vinculacion, organizacion) => new
            {
                nombre = organizacion.Nombre,
                id = vinculacion.VinculacionId
            });
        return new JsonResult(vinculaciones.ToJson());
    }
}