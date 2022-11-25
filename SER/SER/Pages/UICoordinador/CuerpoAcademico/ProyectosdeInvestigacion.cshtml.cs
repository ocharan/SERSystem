using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.Entities;
using SER.DTO;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class ProyectosdeInvestigacion : PageModel
{
    private readonly MySERContext _context;

    public List<Proyectos> listaProyectos { get; set; }
    public List<Pladeafei> Pladeafeis { get; set; }
    public List<VinculacionOrg> VinculacionOrgs { get; set; }

    public ProyectosdeInvestigacion(MySERContext context)
    {
        _context = context;
        listaProyectos = new List<Proyectos>();
        Pladeafeis = new List<Pladeafei>();
        VinculacionOrgs = new List<VinculacionOrg>();
    }
    
    public void OnGet()
    { }
    public JsonResult OnGetObtenerProyectos()
    {
        getProyectos();
        return new JsonResult(listaProyectos.ToJson());
    }
    
    public JsonResult OnGetObtenerPladea()
    {
        getPladeas();
        return new JsonResult(Pladeafeis.ToJson());
    }

    public JsonResult OnGetObtenerVinculacion()
    {
        getVinculacion();
        return new JsonResult(VinculacionOrgs.ToJson());
    }
    
    public void getVinculacion()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaOrg = _context.Organizacions.ToList();
        var vinculaciones = listaVinculacion.Join(listaOrg, vinculacion => vinculacion.OrganizacionIid,
            organizacion => organizacion.OrganizacionId, (vinculacion, organizacion) => new
            {
                vinculacionId = vinculacion.VinculacionId,
                organizacion = organizacion.Nombre,
                fechaConvenio = vinculacion.FechaDeInicioDeConvenio
            });
        foreach (var vin in vinculaciones)
        {
            VinculacionOrg vinculacionOrg = new VinculacionOrg()
            {
                organizacion = vin.organizacion,
                vinculacionId = vin.vinculacionId.ToString(),
                fechaConvenio = vin.fechaConvenio.Value.Date.ToString("d")
            };
            VinculacionOrgs.Add(vinculacionOrg);
        }
    }

    public void getPladeas()
    {
        var listaPladeas = _context.Pladeafeis.ToList();
        foreach (var pladea in listaPladeas)
        {
            Pladeafei pladeafei = new Pladeafei()
            {
                Accion = pladea.Accion,
                PladeafeiId = pladea.PladeafeiId,
                Periodo = pladea.Periodo,
                ObjetivoGeneral = pladea.ObjetivoGeneral
            };
            Pladeafeis.Add(pladeafei);
        }
    }

    public void getProyectos()
    {
        var listaInvestigacion = _context.ProyectoDeInvestigacions.ToList();
        foreach (var investigacion in listaInvestigacion)
        {
            Proyectos proyectos = new Proyectos()
            {
                nombreProyecto = investigacion.Nombre,
                tipoProyecto = "Proyecto de investigacion",
                idProyecto = investigacion.ProyectoDeInvestigacionId,
                fechaInicio = investigacion.FechaInicio.Value.Date.ToString("d")
            };
            listaProyectos.Add(proyectos);
        }
    }
}