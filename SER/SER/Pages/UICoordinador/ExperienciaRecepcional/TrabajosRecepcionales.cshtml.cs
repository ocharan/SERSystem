using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class TrabajosRecepcionales : PageModel
{
    
    private readonly MySERContext _context;
    public List<TrabajoRecepcional> trabajoRecepcionals { get; set; }

    public TrabajosRecepcionales(MySERContext context)
    {
        _context = context;
        trabajoRecepcionals = new List<TrabajoRecepcional>();

    }
    public void OnGet()
    {
        try
        {
            getTrabajosRecepcionales();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void OnPost()
    {
        
    }
    
    public void OnPostBuscar()
    {
        Console.WriteLine("Buscar");
    }
 
    public IActionResult OnPostEstudiantes()
    {
        Console.WriteLine(Request.Query["id"]);
        var id = Request.Query["id"];
        return Redirect("AsignarAlumnos?id="+id);
    }


    public void getTrabajosRecepcionales()
    {
        var trabajosRegistrados = _context.TrabajoRecepcionals.ToList();
        foreach (var trabajo in trabajosRegistrados)
        {
            TrabajoRecepcional trabajoRecepcional = new TrabajoRecepcional()
            {
                Nombre = trabajo.Nombre,
                Modalidad = trabajo.Modalidad,
                Estado = trabajo.Estado,
                Fechadeinicio = trabajo.Fechadeinicio,
                Duracion = trabajo.Duracion,
                TrabajoRecepcionalId = trabajo.TrabajoRecepcionalId
            };
            trabajoRecepcionals.Add(trabajoRecepcional);
        }
    }
}