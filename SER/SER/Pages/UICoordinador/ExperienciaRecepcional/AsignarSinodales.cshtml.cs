using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.DBContext;
using SER.DTO;
using SER.Entidades;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class AsignarSinodales : PageModel
{
    private readonly MySERContext _context;
    
    public List<SinodalDelTrabajo> SinodalesList { get; set; }
    [BindProperty] public List<SinodalAsignado> SinodalAsignados { get; set; }

    public AsignarSinodales(MySERContext context)
    {
        _context = context;
        SinodalesList = new List<SinodalDelTrabajo>();
        SinodalAsignados = new List<SinodalAsignado>();
    }
    
    public void OnGet()
    {
        cargarSinodalesAsignados();
        cargarSinodales(Request.Query["id"]);
    }

    public void cargarSinodalesAsignados()
    {
        var listaSinodales = _context.SinodalDelTrabajos.ToList();
        var listaSinodalesTrabajo = _context.TrabajoRecepcionalSinodalDelTrabajos.ToList();
        var listaSinodalesAsignados = listaSinodalesTrabajo.Join(listaSinodales,
            sinodalAsignado => sinodalAsignado.SinodalDelTrabajoId,
            sinodal => sinodal.SinodalDelTrabajoId, ((sinodalTrabajo, sinodalAsignado) => new
            {
                Nombre = sinodalAsignado.Nombre,
                Tipo = sinodalTrabajo.TipoSinodal,
                idTrabajo = sinodalTrabajo.TrabajoRecepcionalId,
                idSinodal = sinodalAsignado.SinodalDelTrabajoId
            }));
        foreach (var sinodalAsignado in listaSinodalesAsignados)
        {
            SinodalAsignado sinodal = new SinodalAsignado()
            {
                nombre = sinodalAsignado.Nombre,
                tipo = sinodalAsignado.Tipo,
                trabajoId = sinodalAsignado.idTrabajo,
                idSinodal = sinodalAsignado.idSinodal
            };
            SinodalAsignados.Add(sinodal);
        }
    }

    public JsonResult OnGetObtenerSinodales(string idProyecto)
    {
        cargarSinodales(idProyecto);
        return new JsonResult(SinodalesList.ToJson());
    }

    public IActionResult OnPostGuardarRegistro(string SinodalId, string idTrabajo, string Tipo)
    {
        try
        {
            TrabajoRecepcionalSinodalDelTrabajo sinodalAsignado = new TrabajoRecepcionalSinodalDelTrabajo()
            {
                SinodalDelTrabajoId = Int32.Parse(SinodalId),
                TrabajoRecepcionalId = Int32.Parse(idTrabajo),
                TipoSinodal = Tipo
            };
            _context.TrabajoRecepcionalSinodalDelTrabajos.Add(sinodalAsignado);
            _context.SaveChanges();
            return new JsonResult(new { success = true, responseText = "Sinodales registrados correctamente" });
        }
        catch (Exception e)
        {
            return new JsonResult(new { success = false, responseText = "Ha ocurrido un erorr al registrar la información, "+e.Message });
        }
    }

    public void cargarSinodales(string idProyecto)
    {
        var listaSinodalesAsignados = _context.TrabajoRecepcionalSinodalDelTrabajos
            .Where(s => s.TrabajoRecepcionalId == Int32.Parse(idProyecto)).ToList();
        var listaSinodales = _context.SinodalDelTrabajos.ToList();
        foreach (var sinodal in listaSinodales)
        {
            var existe = listaSinodalesAsignados.Any(s => s.SinodalDelTrabajoId.Equals(sinodal.SinodalDelTrabajoId));
            if (!existe)
            {
                SinodalDelTrabajo sinodalDelTrabajo = new SinodalDelTrabajo()
                {
                    SinodalDelTrabajoId = sinodal.SinodalDelTrabajoId,
                    Nombre = sinodal.Nombre,
                };
                SinodalesList.Add(sinodalDelTrabajo);
            }
        }
    }
}