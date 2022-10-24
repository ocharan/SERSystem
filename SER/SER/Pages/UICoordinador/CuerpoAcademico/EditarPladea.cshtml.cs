using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarPladea : PageModel
{

    private readonly MySERContext _context;

    [BindProperty] public string fechaInicio { get; set; }
    [BindProperty] public string fechaFin { get; set;}
    public string idPladea { get; set; }
    [BindProperty] public Pladeafei pladeaRegistrar { get; set; }

    public EditarPladea(MySERContext context)
    {
        _context = context;
        pladeaRegistrar = new Pladeafei();
        fechaInicio = "";
        fechaFin = "";
    }

    public void OnGet()
    {
        obtenerPladea();
    }

    public void OnPost()
    {
        try
        {
            if (Int32.Parse(fechaInicio) < Int32.Parse(fechaFin))
            {
                idPladea = Request.Query["id"];
                pladeaRegistrar.Periodo = fechaInicio + " - " + fechaFin;
                var pladea = _context.Pladeafeis.FirstOrDefault(p => p.PladeafeiId == Int32.Parse(idPladea));
                bool existe =
                    _context.Pladeafeis.Any(p => p.Accion == pladeaRegistrar.Accion && p.Periodo == pladeaRegistrar.Periodo);
                if (pladeaRegistrar.Accion == pladea.Accion)
                {
                    pladea.Periodo = pladeaRegistrar.Periodo;
                    pladea.ObjetivoGeneral = pladeaRegistrar.ObjetivoGeneral;
                    _context.SaveChanges();
                    TempData["Success"] = "Pladea modificada correctamente";
                }else if (pladeaRegistrar.Periodo == pladea.Periodo)
                {
                    pladea.Accion = pladeaRegistrar.Accion;
                    pladea.ObjetivoGeneral = pladeaRegistrar.ObjetivoGeneral;
                    _context.SaveChanges();
                    TempData["Success"] = "Pladea modificada correctamente";
                }
                else
                {
                    if (!existe)
                    {
                        pladea.Accion = pladeaRegistrar.Accion;
                        pladea.Periodo = pladeaRegistrar.Periodo;
                        pladea.ObjetivoGeneral = pladeaRegistrar.ObjetivoGeneral;
                        _context.SaveChanges();
                        TempData["Success"] = "Pladea modificada correctamente";
                    }
                    else
                    {
                        TempData["Error"] = "La información del pladea ha modificar ya existe";
                    }
                }
            }
            else
            {
                TempData["Error"] = "La fecha del periodo final no puede ser menor a la fecha de inicio";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = e.Message;
        }
    }

    public void obtenerPladea()
    {
        idPladea = Request.Query["id"];
        var pladea = _context.Pladeafeis.FirstOrDefault(p => p.PladeafeiId == Int32.Parse(idPladea));
        pladeaRegistrar.Accion = pladea.Accion;
        string[] fecha = pladea.Periodo.Split("-");
        fechaInicio = fecha[0].Trim();
        fechaFin = fecha[1].Trim();
        pladeaRegistrar.ObjetivoGeneral = pladea.ObjetivoGeneral;
    }
}