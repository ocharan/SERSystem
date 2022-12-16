using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class EditarExperienciaEducativa : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty] public ExperienciaEducativa ExperienciaEducativa { get; set; }
    [BindProperty] public bool estado { get; set; }

    [BindProperty] public Archivo ArchivoExperiencia { get; set; }
    
    [BindProperty] public bool archivoExistente { get; set; }

    private IWebHostEnvironment Environment;
    public EditarExperienciaEducativa(MySERContext context, IWebHostEnvironment _environment)
    {
        _context = context;
        ExperienciaEducativa = new ExperienciaEducativa();
        estado = true;
        Environment = _environment;
        ArchivoExperiencia = new Archivo();
        archivoExistente = false;
    }
    
    public void OnGet()
    {
        cargarExperiencia();
    }

    public IActionResult OnPostFile()
    {
        var id = Request.Query["id"];
        var fileExp = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(id));
        return File(fileExp.Direccion, fileExp.TipoContenido);
    }
    
    public async Task<IActionResult> OnPostUpdate(IFormFile? fileExperiencia)
    {
        try
        {
            var experiencia = _context.ExperienciaEducativas.FirstOrDefault(e =>
                e.ExperienciaEducativaId == Int32.Parse(Request.Query["id"]));
            experiencia.Nombre = Request.Form["expNombre"];
            experiencia.Periodo = Request.Form["expPeriodo"];
            experiencia.Seccion = Request.Form["expSeccion"];
            if (Request.Form["expEstado"].ToString().Length == 0)
            {
                experiencia.EstadoAbierto = 1;
            }
            else
            {
                experiencia.EstadoAbierto = 0;
            }
            _context.SaveChanges();
            TempData["Success"] = "Se ha modificado la información correctamente";
            if (fileExperiencia != null)
            {
                ArchivoExperiencia = new Archivo();
                borrarArchivoExperiencia(experiencia.ExperienciaEducativaId);
                string fecha = DateTime.Now.ToString().Replace("/", "");
                string fileName = experiencia.Nrc+"_" + fecha.Replace(" ", "").Replace(":", "");
                var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                using (var fileStream = new FileStream(archivo, FileMode.Create))
                {
                    await fileExperiencia.CopyToAsync(fileStream);
                }
                ArchivoExperiencia.NombreArchivo = fileName +"."+fileExperiencia.ContentType.Split("/")[1];
                ArchivoExperiencia.IdFuente = experiencia.ExperienciaEducativaId;
                ArchivoExperiencia.Direccion = "Archivos/" + fileName;
                ArchivoExperiencia.TipoContenido = fileExperiencia.ContentType;
                ArchivoExperiencia.Fuente = "experiencias";
                _context.Archivos.Add(ArchivoExperiencia);
                _context.SaveChanges();
                TempData["Success"] = "Se modificado la información correctamente";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al modificar la información, " + e.Message;
        }
        return Page();
    }

    public void borrarArchivoExperiencia(int idExperiencia)
    {
        var archivo = _context.Archivos.FirstOrDefault(a => a.IdFuente == idExperiencia && a.Fuente.Equals("experiencias"));
        if (archivo!=null)
        {
            System.IO.File.Delete(Environment.WebRootPath+"/"+archivo.Direccion);
            _context.Remove(archivo);
            _context.SaveChanges();
        }
    }
    
    public void cargarExperiencia()
    {
        var experiencia =
            _context.ExperienciaEducativas.FirstOrDefault(e => e.ExperienciaEducativaId == Int32.Parse(Request.Query["id"]));
        var archivoExperiencia = _context.Archivos.FirstOrDefault(a => a.IdFuente == experiencia.ExperienciaEducativaId && a.Fuente.Equals("experiencias"));
        
        if (archivoExperiencia != null)
        {
            archivoExistente = true;
            ArchivoExperiencia = new Archivo();
            ArchivoExperiencia.NombreArchivo = archivoExperiencia.NombreArchivo;
            ArchivoExperiencia.Direccion = archivoExperiencia.Direccion;
        }
        ExperienciaEducativa.Nombre = experiencia.Nombre;
        ExperienciaEducativa.Nrc = experiencia.Nrc;
        ExperienciaEducativa.Periodo = experiencia.Periodo;
        ExperienciaEducativa.Seccion = experiencia.Seccion;
        if (experiencia.EstadoAbierto == 1)
        {
            estado = false;
        }
        else
        {
            estado = true;
        }
    }
}