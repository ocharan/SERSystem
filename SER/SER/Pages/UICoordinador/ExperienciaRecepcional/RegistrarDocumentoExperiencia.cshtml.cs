using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class RegistrarDocumentoExperiencia : PageModel
{

    private readonly MySERContext _context;
    public List<TipoDocumento> TipoDocumentos { get; set; }
    [BindProperty] public Documento documento { get; set; }
    
    public Archivo archivoExperiencia { get; set; }
    private IWebHostEnvironment Environment;
    public RegistrarDocumentoExperiencia(MySERContext context,  IWebHostEnvironment _environment)
    {
        _context = context;
        Environment = _environment;
        TipoDocumentos = new List<TipoDocumento>();
        documento = new Documento();
        archivoExperiencia = new Archivo();
    }

    public void OnGet()
    {
        try
        {
            getTiposDocumento();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información de registro,"+e.Message;
        }
    }

    public async Task<IActionResult> OnPost(IFormFile fileExperiencia)
    {
        try
        {
            var id = Request.Query["id"];
            var tipo = _context.TipoDocumentos.FirstOrDefault(t => t.IdTipo == Int32.Parse(Request.Form["tipo"]));
            bool existe = _context.Documentos.Any(d => d.TipoDocumentoId == tipo.IdTipo && d.TrabajoRecepcionalId == Int32.Parse(id));
            if (!existe)
            {
                if (fileExperiencia != null)
                {
                    var nombreDocumento = tipo.NombreDocumento.Replace(" ", "");
                    documento.TrabajoRecepcionalId = Int32.Parse(id);
                    string fecha = DateTime.Now.ToString().Replace("/", "");
                    string fileName = nombreDocumento+"_" + fecha.Replace(" ", "").Replace(":", "") + id;
                    var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                    using (var fileStream = new FileStream(archivo, FileMode.Create))
                    {
                        await fileExperiencia.CopyToAsync(fileStream);
                    }
                    archivoExperiencia.NombreArchivo = fileName;
                    archivoExperiencia.IdFuente = Int32.Parse(id);
                    archivoExperiencia.Direccion = "Archivos/" + fileName;
                    archivoExperiencia.Fuente = tipo.NombreDocumento;
                    archivoExperiencia.TipoContenido = fileExperiencia.ContentType;
                    documento.TipoDocumentoId = tipo.IdTipo;
                    _context.Documentos.Add(documento);
                    _context.Archivos.Add(archivoExperiencia);
                    _context.SaveChanges();
                    TempData["Success"] = "El documento se ha subido correctamente";
                }
                else
                {
                    TempData["Error"] = "Debes de seleccionar un archivo";
                }
            }
            else
            {
                TempData["Error"] = "El documento que intentas subir ya esta registrado";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error " + e.Message;
        }
        return Page();
    }

    public void getTiposDocumento()
    {
        var listaTipos = _context.TipoDocumentos.ToList().Where(t=> t.ExperienciaEducativa=="Experiencia Recepcional");
        foreach (var tipo in listaTipos)
        {
            TipoDocumento tipoDocumento = new TipoDocumento()
            {
                NombreDocumento = tipo.NombreDocumento,
                IdTipo = tipo.IdTipo
            };
            TipoDocumentos.Add(tipoDocumento);
        }
    }
}

