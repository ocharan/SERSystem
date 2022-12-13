using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class RegistrarSinodalInterno : PageModel
{
    private readonly MySERContext _context;
    
    [BindProperty]
    public SinodalDelTrabajo SinodalDelTrabajo { get; set; }
    public List<Organizacion> Organizacions { get; set; }
    public List<Profesor> Profesors { get; set; }
    
    
    public RegistrarSinodalInterno(MySERContext context)
    {
        _context = context;
        Organizacions = new List<Organizacion>();
        Profesors = new List<Profesor>();
    }
    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }
    
    public void OnGet()
    {
        try
        {
            cargarOrganizaciones();
            cargarProfesores();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar la información de registro, "+e.Message;
        }
    }
    
    public void OnPost()
    {
        try
        {
            SinodalDelTrabajo.OrganizacionId = Convert.ToInt32(Request.Form["OrgId"]);
            var sinodalesExistentes = _context.SinodalDelTrabajos.ToList();
            SinodalDelTrabajo.Nombre = SinodalDelTrabajo.Nombre?.ToUpper();
            bool sinodalYaExiste = sinodalesExistentes.Any(s => s.NumeroDePersonal == SinodalDelTrabajo.NumeroDePersonal && s.CorreoElectronico == SinodalDelTrabajo.CorreoElectronico);
            if (sinodalYaExiste)
            {
                TempData["ErrorMessage"] = "El profesor seleccionado ya se encuentra registrado como sinodal";
            }
            else
            {
                _context.SinodalDelTrabajos.Add(SinodalDelTrabajo);
                _context.SaveChanges();
                TempData["Success"] = "Sinodal registrado correctamente";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al completar el registro " + e.Message;
        }   
    }

    public void cargarProfesores()
    {
        try
        {
            var listaProfesores = _context.Profesors.ToList();
            foreach (var profe in listaProfesores)
            {
                Profesor profesor = new Profesor()
                {
                    Nombre = profe.Nombre,
                    ProfesorId = profe.ProfesorId,
                    NumeroDePersonal = profe.NumeroDePersonal
                };
                Profesors.Add(profesor);
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar los profesores " + e.Message;

        }
    }
    
    public void cargarOrganizaciones()
    {
        try
        {
            var listaOrgs = _context.Organizacions.ToList();
            foreach (var org in listaOrgs)
            {
                Organizacion organizacion = new Organizacion()
                {
                    Nombre = org.Nombre,
                    OrganizacionId = org.OrganizacionId
                };
                Organizacions.Add(organizacion);
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Error al cargar las organizaciones";
        }
    }
}