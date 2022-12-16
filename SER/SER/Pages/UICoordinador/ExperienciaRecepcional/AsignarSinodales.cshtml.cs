using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.Entities;
using SER.DTO;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;
[Authorize(Roles = "Coordinador")]
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
        cargarSinodales(Request.Query["id"]);
    }
    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }

    public JsonResult OnPostEliminarSinodal(string idSinodal, string idTrabajo)
    {
        try
        {
            var sinodal = _context.TrabajoRecepcionalSinodalDelTrabajos.FirstOrDefault(t =>
                t.SinodalDelTrabajoId == Int32.Parse(idSinodal) &&
                t.TrabajoRecepcionalId == Int32.Parse(idTrabajo));
            _context.Remove(sinodal);
            _context.SaveChanges();
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            return new JsonResult(new { success = false});
        }
    }

    public void cargarSinodalesAsignados(string idProyecto)
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
            if (sinodalAsignado.idTrabajo == Int32.Parse(idProyecto))
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
    }

    public JsonResult OnGetObtenerSinodalesAsignados(string idProyecto)
    {
        cargarSinodalesAsignados(idProyecto);
        return new JsonResult(SinodalAsignados.ToJson());
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
            return new JsonResult(new { success = true});
        }
        catch (Exception e)
        {
            return new JsonResult(new { success = false});
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