using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

[Authorize(Roles = "Coordinador")]
public class EditarProyectoDeInvestigacion : PageModel
{

    private readonly MySERContext _context;
    
    public string idProyecto { get; set; }
    public List<Entities.CuerpoAcademico> CuerposAcademicosList { get; set; }

    [BindProperty]
    public ProyectoDeInvestigacion ProyectodeInvestigacion { get; set; }
    public EditarProyectoDeInvestigacion(MySERContext context)
    {
        _context = context;
        CuerposAcademicosList = new List<Entities.CuerpoAcademico>();
        ProyectodeInvestigacion = new ProyectoDeInvestigacion();
    }

    public void OnPost()
    {
        idProyecto = Request.Query["id"];
        try
        {
            var proyectosExistentes = _context.ProyectoDeInvestigacions.ToList();
            var proyecto = _context.ProyectoDeInvestigacions.First(c => c.ProyectoDeInvestigacionId == Int32.Parse(idProyecto));
            bool existeProyecto = proyectosExistentes.Any(c => c.Nombre.Equals(ProyectodeInvestigacion.Nombre));
            if (proyecto.Nombre == ProyectodeInvestigacion.Nombre)
            {
                proyecto.CuerpoAcademicoId = ProyectodeInvestigacion.CuerpoAcademicoId;
                proyecto.FechaInicio = ProyectodeInvestigacion.FechaInicio;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Proyecto de investigación actualizado correctamente";
            }
            else
            {
                if (!existeProyecto)
                {
                    proyecto.Nombre = ProyectodeInvestigacion.Nombre;
                    proyecto.FechaInicio = ProyectodeInvestigacion.FechaInicio;
                    proyecto.CuerpoAcademicoId = ProyectodeInvestigacion.CuerpoAcademicoId;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Proyecto de investigación actualizado correctamente";
                }else
                {
                    TempData["ErrorMessage"] = "El nombre del proyecto ya esta registrado";
                }
            }
            
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = e.Message;
        }
    }
    
    public void OnGet()
    {
        try
        {
            idProyecto = Request.Query["id"];
            cargarCuerpos();
            obtenerProyecto();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar la información del proyecto de investigación.";
        }
    }

    public void obtenerProyecto()
    {
        var proyecto =
            _context.ProyectoDeInvestigacions.FirstOrDefault(
                p => p.ProyectoDeInvestigacionId == Int32.Parse(idProyecto));
        ProyectodeInvestigacion.Nombre = proyecto.Nombre;
        ProyectodeInvestigacion.FechaInicio = proyecto.FechaInicio;
        ProyectodeInvestigacion.CuerpoAcademicoId = proyecto.CuerpoAcademicoId;
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
            CuerposAcademicosList.Add(cuerpoNuevo);
        }
    }
}