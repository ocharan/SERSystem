using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;
using SER.DTO;
using Lgac = SER.Entities.Lgac;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistroProyectoDeInvestigacion : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty]
    public ProyectoDeInvestigacion ProyectoDeInvestigacion { get; set; }
    public List<Entities.CuerpoAcademico> CuerpoAcademicos { get; set; }
    
    public List<Lgac> Lgacs { get; set; }

    public RegistroProyectoDeInvestigacion(MySERContext context)
    {
        _context = context;
        CuerpoAcademicos = new List<Entities.CuerpoAcademico>();
        Lgacs = new List<Lgac>();
    }
    
    public void OnGet()
    {
        cargarCuerpos();
    }

    public void OnPost()
    {
        try
        {
            var proyectos = _context.ProyectoDeInvestigacions.ToList();
            bool existe = proyectos.Any(e => e.Nombre.Equals(ProyectoDeInvestigacion.Nombre) && e.CuerpoAcademicoId==ProyectoDeInvestigacion.CuerpoAcademicoId);
            if (!existe)
            {
                if (ProyectoDeInvestigacion.CuerpoAcademicoId == 0)
                {
                    TempData["ErrorMessage"] = "Debe de seleccionar un cuerpo academico";
                    cargarCuerpos();
                }
                else
                {
                    _context.ProyectoDeInvestigacions.Add(ProyectoDeInvestigacion);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Proyecto registrado correctamente";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "El proyecto que intenta registrar ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = e.Message;   
        }
    }
    
    public void cargarCuerpos()
    {
        var listaCuerpos = _context.CuerpoAcademicos.ToList();
        foreach (var cuerpo in listaCuerpos)
        {
            Entities.CuerpoAcademico cuerpoNuevo = new Entities.CuerpoAcademico()
            {
                Nombre = cuerpo.Nombre,
                CuerpoAcademicoId = cuerpo.CuerpoAcademicoId
            };
            CuerpoAcademicos.Add(cuerpoNuevo);
        }
    }
    
    public void cargarLgacs()
    {
        var listaLgacs = _context.Lgacs.ToList();
        foreach (var lgac in listaLgacs)
        {
            Lgac lgacNuevo = new Lgac()
            {
                Nombre = lgac.Nombre,
                LgacId = lgac.LgacId,
                CuerpoAcademicoId = lgac.CuerpoAcademicoId
            };
            Lgacs.Add(lgacNuevo);
        }
    }
}