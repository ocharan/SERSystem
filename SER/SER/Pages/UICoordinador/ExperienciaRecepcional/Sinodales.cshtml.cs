using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;
using SER.DTO;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class Sinodales : PageModel
{

    private readonly MySERContext _context;
    public List<SinodalVista> sinodales { get; set; }
    
    [BindProperty]
    public SinodalDelTrabajo SinodalDelTrabajo { get; set; }

    public Sinodales(MySERContext context)
    {
        _context = context;
        sinodales = new List<SinodalVista>();
    }

    public void OnGet()
    {
        try
        {
            getSinodales();
            
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar los sinodales, "+ex.Message;
        }
    }

    public IActionResult OnPostModificarInterno()
    {
        return Redirect("ModificarSinodalInterno?id="+Request.Query["id"]);
    }
    
    public IActionResult OnPostModificarExterno()
    {
        return Redirect("ModificarSinodalExterno?id="+Request.Query["id"]);
    }


    public void getSinodales()
    {
        var sinodalesRegistrados = _context.SinodalDelTrabajos.ToList();
        var organizaciones = _context.Organizacions.ToList();
        var resultadoVista = sinodalesRegistrados.Join(organizaciones, sinodal => sinodal.OrganizacionId,
            organizacion => organizacion.OrganizacionId, ((trabajo, organizacion) => new
            {
                Nombre = trabajo.Nombre,
                CorreoElectronico = trabajo.CorreoElectronico,
                Telefono = trabajo.Telefono,
                OrganizacionNombre = organizacion.Nombre,
                SinodalId = trabajo.SinodalDelTrabajoId,
                numeroPersonal = trabajo.NumeroDePersonal
            }));
        foreach (var sinodal in resultadoVista)
        {
            SinodalVista sinodalRegistrado = new SinodalVista();
            sinodalRegistrado.nombre = sinodal.Nombre;
            sinodalRegistrado.correo = sinodal.CorreoElectronico;
            sinodalRegistrado.telefono = sinodal.Telefono;
            sinodalRegistrado.organizacion = sinodal.OrganizacionNombre;
            sinodalRegistrado.id = sinodal.SinodalId;
            sinodalRegistrado.numeroPersonal = sinodal.numeroPersonal.ToString();
            sinodales.Add(sinodalRegistrado);
        }
    }

    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }
    
}