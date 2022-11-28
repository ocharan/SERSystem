using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.DTO;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class ModificarTrabajoRecepcional : PageModel
{
    private readonly MySERContext _context;

    [BindProperty] public TrabajoRecepcional TrabajoRecepcional { get; set; }
    
    [BindProperty] public string tipoTrabajo { get; set; }
    public string proyectoAsociado { get; set; }
    
    public List<Pladeafei> Pladeafeis { get; set; }

    public List<VinculacionOrg> Vinculacions { get; set; }
    
    public List<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }

    public ModificarTrabajoRecepcional(MySERContext context)
    {
        _context = context;
        TrabajoRecepcional = new TrabajoRecepcional();
        Pladeafeis = new List<Pladeafei>();
        Vinculacions = new List<VinculacionOrg>();
        ProyectoDeInvestigacions = new List<ProyectoDeInvestigacion>();
    }
    
    
    public void OnGet()
    {
        cargarInformacionTrabajo();
    }

    public void cargarInformacionTrabajo()
    {
        var trabajo =
            _context.TrabajoRecepcionals.FirstOrDefault(t =>
                t.TrabajoRecepcionalId == Int32.Parse(Request.Query["id"]));
        TrabajoRecepcional.Nombre = trabajo.Nombre;
        TrabajoRecepcional.Duracion = trabajo.Duracion;
        TrabajoRecepcional.Fechadeinicio = trabajo.Fechadeinicio;
        TrabajoRecepcional.LineaDeInvestigacion = trabajo.LineaDeInvestigacion;
        TrabajoRecepcional.Modalidad = trabajo.Modalidad;
        if (trabajo.PladeafeiId > -1)
        {
            proyectoAsociado = "pladea";
            cargarPladeas();
            TrabajoRecepcional.PladeafeiId = trabajo.PladeafeiId;
        }else if (trabajo.VinculacionId > -1)
        {
            proyectoAsociado = "vinculacion";
            cargarVinculacion();
            TrabajoRecepcional.VinculacionId = trabajo.VinculacionId;
        }else if (trabajo.ProyectoDeInvestigacionId > -1)
        {
            proyectoAsociado = "investigacion";
            cargarInvestigacion();
            TrabajoRecepcional.ProyectoDeInvestigacionId = trabajo.ProyectoDeInvestigacionId;
        }
    }

    public void cargarPladeas()
    {
        Pladeafeis = _context.Pladeafeis.ToList();
    }

    public void cargarVinculacion()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaOrgs = _context.Organizacions.ToList();
        var vinculaciones = listaVinculacion.Join(listaOrgs, vinculacion => vinculacion.OrganizacionIid,
            organizacion => organizacion.OrganizacionId, (vinculacion, organizacion) => new
            {
                nombre = organizacion.Nombre,
                id = vinculacion.VinculacionId
            });
        foreach (var vinculacion in vinculaciones)
        {
            VinculacionOrg vinculacionOrg = new VinculacionOrg()
            {
                organizacion = vinculacion.nombre,
                vinculacionId = vinculacion.id.ToString()
            };
            Vinculacions.Add(vinculacionOrg);
        }
    }

    public void cargarInvestigacion()
    {
        ProyectoDeInvestigacions = _context.ProyectoDeInvestigacions.ToList();
    }
}