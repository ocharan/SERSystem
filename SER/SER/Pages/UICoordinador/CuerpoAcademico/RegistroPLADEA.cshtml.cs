using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using System.IO;
using SER.DTO;
using SER.Entidades;
using Lgac = SER.Entidades.Lgac;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistroPLADEA : PageModel
{

    private readonly MySERContext _context;
    [BindProperty] public Pladeafei pladeaRegistrar { get; set; }
    private Archivo ArchivoPladea { get; set; }
    private IWebHostEnvironment Environment;

    public RegistroPLADEA(MySERContext context,  IWebHostEnvironment _environment)
    {
        _context = context;
        Environment = _environment;
        ArchivoPladea = new Archivo();
    }

    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPost(IFormFile? file)
    {
        try
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
                        if (file != null)
                        {
                            string fecha = DateTime.Now.ToString().Replace("/", "");
                            string fileName = "PLADEA_" + fecha.Replace(" ", "").Replace(":", "");
                            var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                            using (var fileStream = new FileStream(archivo, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            ArchivoPladea.NombreArchivo = fileName +"."+file.ContentType.Split("/")[1];
                            ArchivoPladea.IdFuente = pladeaRegistrar.PladeafeiId;
                            ArchivoPladea.Direccion = "Archivos/" + fileName;
                            ArchivoPladea.TipoContenido = file.ContentType;
                            _context.Archivos.Add(ArchivoPladea);
                            _context.SaveChanges();

                        }
                        TempData["SuccessMessage"] = "Registro completado";
                        return Page();
                    }
                }
                else
                {
                    TempData["ErrorDateMessage"] = "La fecha de finalización no puede ser menor a la fecha de inicio";
                    return Page();
                }
            }
        }
        catch (Exception e)
        {
            TempData["ErrorDateMessage"] = "Ha ocurrido un error al registrar la información, "+e.Message;
            return Page();
        }
        return Page();
    }
}