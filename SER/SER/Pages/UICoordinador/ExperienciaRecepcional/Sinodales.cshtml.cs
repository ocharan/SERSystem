using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.DTO;
using SER.Entidades;

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
            Console.WriteLine(ex.Message);
        }
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
                SinodalId = trabajo.SinodalDelTrabajoId
            }));
        foreach (var sinodal in resultadoVista)
        {
            SinodalVista sinodalRegistrado = new SinodalVista();
            sinodalRegistrado.nombre = sinodal.Nombre;
            sinodalRegistrado.correo = sinodal.CorreoElectronico;
            sinodalRegistrado.telefono = sinodal.Telefono;
            sinodalRegistrado.organizacion = sinodal.OrganizacionNombre;
            sinodalRegistrado.id = sinodal.SinodalId;
            sinodales.Add(sinodalRegistrado);
        }
    }
}