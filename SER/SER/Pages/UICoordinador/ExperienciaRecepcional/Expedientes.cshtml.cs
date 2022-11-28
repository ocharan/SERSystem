using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

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
        cargarExpedientes();
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
}