using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

[Authorize(Roles = "Coordinador")]
public class EditarPladea : PageModel
{

    private readonly MySERContext _context;

    [BindProperty] public string fechaInicio { get; set; }
    [BindProperty] public string fechaFin { get; set;}
    public string idPladea { get; set; }
    [BindProperty] public Pladeafei pladeaRegistrar { get; set; }
    
    [BindProperty] public Archivo ArchivoPladea { get; set; }

    private IWebHostEnvironment Environment;

    public EditarPladea(MySERContext context, IWebHostEnvironment _environment)
    {
        _context = context;
        pladeaRegistrar = new Pladeafei();
        fechaInicio = "";
        fechaFin = "";
        Environment = _environment;
    }

    public void OnGet()
    {
        try
        {
            obtenerPladea();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información, " + e.Message;
        }
    }

    public async Task<IActionResult> OnPostUpdate(IFormFile? filePladea)
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
                
                if (filePladea != null)
                {
                    ArchivoPladea = new Archivo();
                    borrarArchivoPladea(Int32.Parse(idPladea));
                    string fecha = DateTime.Now.ToString().Replace("/", "");
                    string fileName = "PLADEA_" + fecha.Replace(" ", "").Replace(":", "");
                    var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                    using (var fileStream = new FileStream(archivo, FileMode.Create))
                    {
                        await filePladea.CopyToAsync(fileStream);
                    }

                    ArchivoPladea.NombreArchivo = fileName +"."+filePladea.ContentType.Split("/")[1];
                    ArchivoPladea.IdFuente = Int32.Parse(idPladea);
                    ArchivoPladea.Direccion = "Archivos/" + fileName;
                    ArchivoPladea.TipoContenido = filePladea.ContentType;
                    ArchivoPladea.Fuente = "proyectos";
                    _context.Archivos.Add(ArchivoPladea);
                    _context.SaveChanges();
                    TempData["Success"] = "Pladea modificada correctamente con archivo";
                }
            }
            else
            {
                TempData["Error"] = "La fecha del periodo final no puede ser menor a la fecha de inicio";
                return Page();
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = e.StackTrace;
        }
        return Page();
    }

    public IActionResult OnPostFile()
    { 
        var id = Request.Query["id"];
        var filePladea = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(id));
        return File(filePladea.Direccion, filePladea.TipoContenido);
    }
    

    public void borrarArchivoPladea(int idPladea)
    {
        var archivo = _context.Archivos.FirstOrDefault(a => a.IdFuente == idPladea);
        if (archivo!=null)
        {
            System.IO.File.Delete(Environment.WebRootPath+"/"+archivo.Direccion);
            _context.Remove(archivo);
            _context.SaveChanges();
        }
    }
    public void obtenerPladea()
    {
        idPladea = Request.Query["id"];
        var pladea = _context.Pladeafeis.FirstOrDefault(p => p.PladeafeiId == Int32.Parse(idPladea));
        var archivoPladea = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(idPladea) && a.Fuente.Equals("proyectos"));
        pladeaRegistrar.Accion = pladea.Accion;
        string[] fecha = pladea.Periodo.Split("-");
        fechaInicio = fecha[0].Trim();
        fechaFin = fecha[1].Trim();
        pladeaRegistrar.ObjetivoGeneral = pladea.ObjetivoGeneral;
        if (archivoPladea != null)
        {
            ArchivoPladea = new Archivo();
            ArchivoPladea.NombreArchivo = archivoPladea.NombreArchivo;
            ArchivoPladea.Direccion = archivoPladea.Direccion;
        }
    }
}