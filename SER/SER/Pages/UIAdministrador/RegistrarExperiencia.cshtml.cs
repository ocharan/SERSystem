using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class RegistrarExperiencia : PageModel
{

    private readonly MySERContext _context;
    private Archivo ArchivoExperiencia;
    [BindProperty] public ExperienciaEducativa ExperienciaEducativa { get; set; }
    private IWebHostEnvironment Environment;

    public RegistrarExperiencia(MySERContext context, IWebHostEnvironment _environment)
    {
        _context = context;
        Environment = _environment;
        ExperienciaEducativa = new ExperienciaEducativa();
        ArchivoExperiencia = new Archivo();
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(IFormFile? fileExperiencia)
    {
        try
        {
            bool existe = _context.ExperienciaEducativas.Any(e => e.Nrc.Equals(ExperienciaEducativa.Nrc));
            if (!existe)
            {
                ExperienciaEducativa.EstadoAbierto = 1;
                _context.ExperienciaEducativas.Add(ExperienciaEducativa);
                _context.SaveChanges();
                if (fileExperiencia != null)
                {
                    string fecha = DateTime.Now.ToString().Replace("/", "");
                    string fileName = ExperienciaEducativa.Nrc+"_" + fecha.Replace(" ", "").Replace(":", "");
                    var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                    using (var fileStream = new FileStream(archivo, FileMode.Create))
                    {
                        await fileExperiencia.CopyToAsync(fileStream);
                    }
                    ArchivoExperiencia.NombreArchivo = fileName +"."+fileExperiencia.ContentType.Split("/")[1];
                    ArchivoExperiencia.IdFuente = ExperienciaEducativa.ExperienciaEducativaId;
                    ArchivoExperiencia.Direccion = "Archivos/" + fileName;
                    ArchivoExperiencia.TipoContenido = fileExperiencia.ContentType;
                    ArchivoExperiencia.Fuente = "experiencias";
                    _context.Archivos.Add(ArchivoExperiencia);
                    _context.SaveChanges();
                    TempData["Success"] = "Se ha registrado la experiencia edudativa correctamente";
                }
            }
            else
            {
                TempData["Error"] = "La experiencia educativa ya existe, intente con otro NRC";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al intentar registrar la experiencia educativa, "+e.Message;
        }
        return Page();
    }
}