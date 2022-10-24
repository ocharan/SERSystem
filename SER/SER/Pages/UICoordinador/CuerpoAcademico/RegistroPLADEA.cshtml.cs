using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.DTO;
using SER.Entidades;
using Lgac = SER.Entidades.Lgac;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistroPLADEA : PageModel
{

    private readonly MySERContext _context;

    [BindProperty] public Pladeafei pladeaRegistrar { get; set; }

    public RegistroPLADEA(MySERContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        var fechaInicio = Request.Form["FechaInicio"];
        var fechaFinalizacion = Request.Form["FechaFin"];
        if (fechaInicio.Count == 0 || fechaFinalizacion.Count == 0)
        {
            TempData["ErrorDateMessage"] = "Debe de seleccionar una fecha de inicio y finalización valida";
        }
        else
        { 
            if (Int32.Parse(fechaInicio) < Int32.Parse(fechaFinalizacion))
            {
                pladeaRegistrar.Periodo = fechaInicio + " - " + fechaFinalizacion;
                bool existe = _context.Pladeafeis.ToList().Any(p => p.Accion.Equals(pladeaRegistrar.Accion)
                                                                    && p.Periodo.Equals(pladeaRegistrar.Periodo));
                if (existe)
                {
                    TempData["ErrorDateMessage"] = "El Proyecto PLADEA que intenta registrar ya existe";
                }
                else
                {
                    _context.Pladeafeis.Add(pladeaRegistrar);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Registro completado";
                }
            }
            else
            {
                TempData["ErrorDateMessage"] = "La fecha de finalización no puede ser menor a la fecha de inicio";
            }
        }
    }
}