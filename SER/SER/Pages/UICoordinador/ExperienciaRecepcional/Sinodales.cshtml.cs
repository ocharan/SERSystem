using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class Sinodales : PageModel
{

    private readonly MySERContext _context;
    public List<SinodalDelTrabajo> sinodales { get; set; }
    
    [BindProperty]
    public SinodalDelTrabajo SinodalDelTrabajo { get; set; }

    public Sinodales(MySERContext context)
    {
        _context = context;
        sinodales = new List<SinodalDelTrabajo>();
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
        foreach (var sinodal in sinodalesRegistrados)
        {
            SinodalDelTrabajo sinodalDelTrabajo = new SinodalDelTrabajo()
            {
                Nombre = sinodal.Nombre,
                CorreoElectronico = sinodal.CorreoElectronico,
                Telefono = sinodal.Telefono
            };
            sinodales.Add(sinodalDelTrabajo);
        }
    }
}