using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.DTO;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class ProyectosdeInvestigacion : PageModel
{
    private readonly MySERContext _context;

    public List<Proyectos> listaProyectos;

    public ProyectosdeInvestigacion(MySERContext context)
    {
        _context = context;
        listaProyectos = new List<Proyectos>();
    }
    
    public void OnGet()
    {
        cargarProyectos();
    }

    public void cargarProyectos()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaInvestigacion = _context.ProyectoDeInvestigacions.ToList();
        var listaPladea = _context.Pladeafeis.ToList();
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