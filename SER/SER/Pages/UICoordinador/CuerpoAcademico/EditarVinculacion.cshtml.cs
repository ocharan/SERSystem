using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarVinculacion : PageModel
{

    private MySERContext _context;

    public string idVinculacion;

    private IWebHostEnvironment Environment;

    [BindProperty] public Archivo ArchivoVinculacion { get; set; }
    public List<Organizacion> OrganizacionesList { get; set; }

    [BindProperty] public Vinculacion Vinculacion { get; set; }
    
    public EditarVinculacion(MySERContext context, IWebHostEnvironment _environment)
    {
        _context = context;
        OrganizacionesList = new List<Organizacion>();
        Vinculacion = new Vinculacion();
        Environment = _environment;
    }

    public IActionResult OnPostFile()
    {
        var id = Request.Query["id"];
        var fileVinculacion = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(id));
        return File(fileVinculacion.Direccion, fileVinculacion.TipoContenido);
    }

    public async Task<IActionResult> OnPostUpdate(IFormFile? fileVinculacion)
    {
        try
        {
            idVinculacion = Request.Query["id"];
            var vinculacion = _context.Vinculacions.FirstOrDefault(v => v.VinculacionId == Int32.Parse(idVinculacion));
            bool existe = _context.Vinculacions.Any(v => v.OrganizacionIid == Int32.Parse(Request.Form["orgId"]));
            if (Int32.Parse(Request.Form["orgId"]) == vinculacion.OrganizacionIid)
            {
                vinculacion.FechaDeInicioDeConvenio = DateTime.Parse(Request.Form["fechaInicio"]);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Vinculación actualizada correctamente";
            }
            else
            {
                if (!existe)
                {
                    vinculacion.OrganizacionIid = Int32.Parse(Request.Form["orgId"]);
                    vinculacion.FechaDeInicioDeConvenio = DateTime.Parse(Request.Form["fechaInicio"]);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Vinculación actualizada correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "La organización vinculada ya existe";
                }
            }
            if (fileVinculacion != null)
            {
                ArchivoVinculacion = new Archivo();
                borrarArchivoVinculacion(Int32.Parse(idVinculacion));
                string fecha = DateTime.Now.ToString().Replace("/", "");
                string fileName = "VINCULACION_" + fecha.Replace(" ", "").Replace(":", "");
                var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                using (var fileStream = new FileStream(archivo, FileMode.Create))
                {
                    await fileVinculacion.CopyToAsync(fileStream);
                }
                ArchivoVinculacion.NombreArchivo = fileName +"."+fileVinculacion.ContentType.Split("/")[1];
                ArchivoVinculacion.IdFuente = Int32.Parse(idVinculacion);
                ArchivoVinculacion.Direccion = "Archivos/" + fileName;
                ArchivoVinculacion.TipoContenido = fileVinculacion.ContentType;
                _context.Archivos.Add(ArchivoVinculacion);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Vinculación actualizada correctamente con vinculacion";
            }
            cargarOrganizaciones();
            cargarVinculacion();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] =
                "Ha ocurrido un error durante la actualización del proyecto vinculación " + e.StackTrace;
        }
        return Page();
    }
    
    public void OnGet()
    {
        try
        {
            cargarOrganizaciones();
            cargarVinculacion();
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información";
        }
    }

    public void cargarVinculacion()
    {
        idVinculacion = Request.Query["id"];
        var vinculacion = _context.Vinculacions.FirstOrDefault(v => v.VinculacionId == Int32.Parse(idVinculacion));
        var archivoVinculacion = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(idVinculacion));
        Vinculacion.OrganizacionIid = vinculacion.OrganizacionIid;
        Vinculacion.FechaDeInicioDeConvenio = vinculacion.FechaDeInicioDeConvenio;
        if (archivoVinculacion != null)
        {
            ArchivoVinculacion = new Archivo();
            ArchivoVinculacion.NombreArchivo = archivoVinculacion.NombreArchivo;
            ArchivoVinculacion.Direccion = archivoVinculacion.Direccion;
        }
    }
    
    public void borrarArchivoVinculacion(int idVinculacion)
    {
        var archivo = _context.Archivos.FirstOrDefault(a => a.IdFuente == idVinculacion);
        if (archivo!=null)
        {
            System.IO.File.Delete(Environment.WebRootPath+"/"+archivo.Direccion);
            _context.Remove(archivo);
            _context.SaveChanges();
        }
    }
    
    public void cargarOrganizaciones()
    {
        var listaOrg = _context.Organizacions.ToList();
        foreach (var org in listaOrg)
        {
            Organizacion orgExistente = new Organizacion()
            {
                Nombre = org.Nombre,
                OrganizacionId = org.OrganizacionId
            };
            OrganizacionesList.Add(orgExistente);
        }
    }
}