using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class ModificarSinodalInterno : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty] public SinodalDelTrabajo SinodalDelTrabajo { get; set; }
    
    [BindProperty] public List<Organizacion> Organizacions { get; set; }

    public ModificarSinodalInterno(MySERContext context)
    {
        _context = context;
        SinodalDelTrabajo = new SinodalDelTrabajo();
        Organizacions = new List<Organizacion>();
    }
    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }

    public void OnPost()
    {
        try
        {
            var sinodal = _context.SinodalDelTrabajos.First(s => s.SinodalDelTrabajoId == Request.Form["id"]);
            sinodal.OrganizacionId = Int32.Parse(Request.Form["orgId"]);
            sinodal.CorreoElectronico = Request.Form["correoInterno"];
            sinodal.Telefono = Request.Form["telefonoInterno"];
            _context.SaveChanges();
            TempData["Success"] = "Sinodal actualizado correctamente";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar la información del sinodal, " + e.Message;
        }
    }
    
    public void OnGet()
    {
        try
        {
            cargarInfoSinodal();
            cargarOrganizaciones();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar la información, " + e.Message;
        }
    }
    
    public void cargarOrganizaciones()
    {
        var organizaciones = _context.Organizacions.ToList();
        foreach (var org in organizaciones)
        {
            Organizacion organizacion = new Organizacion()
            {
                OrganizacionId = org.OrganizacionId,
                Nombre = org.Nombre
            };
            Organizacions.Add(organizacion);
        }
    }
    
    public void cargarInfoSinodal()
    {
        var sinodal = _context.SinodalDelTrabajos.First(s=>s.SinodalDelTrabajoId == Int32.Parse(Request.Query["id"]));
        SinodalDelTrabajo.CorreoElectronico = sinodal.CorreoElectronico;
        SinodalDelTrabajo.Telefono = sinodal.Telefono;
        SinodalDelTrabajo.OrganizacionId = sinodal.OrganizacionId;
        SinodalDelTrabajo.SinodalDelTrabajoId = sinodal.SinodalDelTrabajoId;
        SinodalDelTrabajo.Nombre = sinodal.Nombre;
        SinodalDelTrabajo.NumeroDePersonal = sinodal.NumeroDePersonal;
    }
}