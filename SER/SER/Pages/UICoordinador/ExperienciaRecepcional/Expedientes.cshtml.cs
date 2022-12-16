using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

[Authorize(Roles = "Coordinador")]
public class Expedientes : PageModel
{
    private readonly MySERContext _context;
    
    public List<Expediente> ExpedientesList { get; set; }

    public Expedientes(MySERContext context)
    {
        _context = context;
        ExpedientesList = new List<Expediente>();
    }
    
    public void OnGet()
    {
        try
        {
            cargarExpedientes();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información solicitada";
        }
    }

    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }
    
    public void cargarExpedientes()
    {
        var listaExpedientes = _context.Expedientes.ToList();
        foreach (var expediente in listaExpedientes)
        {
            Expediente exp = new Expediente()
            {
                NombreAlumno = expediente.NombreAlumno,
                Matricula = expediente.Matricula,
                CorreoElectronico = expediente.CorreoElectronico,
                Nombre = expediente.Nombre,
                Estado = expediente.Estado,
                Modalidad = expediente.Modalidad
            };
            ExpedientesList.Add(exp);
        }
    }

    public IActionResult OnPostExpediente()
    {
        return Redirect("VerExpediente?id="+Request.Query["id"]);
    }
}