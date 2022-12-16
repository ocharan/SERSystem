using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.CuerpoAcademico;
[Authorize(Roles = "Coordinador")]
public class RegistroVinculacion : PageModel
{

    private readonly MySERContext _context;

    private Archivo ArchivoVinculacion { get; set; }
    public Vinculacion Vinculacion { get; set; }
    public List<Organizacion> OrganizacionesList { get; set; }

    private IWebHostEnvironment Environment;
    
    public RegistroVinculacion(MySERContext context, IWebHostEnvironment _environment)
    {
        _context = context;
        OrganizacionesList = new List<Organizacion>();
        Vinculacion = new Vinculacion();
        ArchivoVinculacion = new Archivo();
        Environment = _environment;
    }
    
    public void OnGet()
    {
        try
        {
            cargarOrganizaciones();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Error al cargar la información de registro";
        }
    }

    public async Task<IActionResult> OnPost(IFormFile? fileVinculacion)
    {
        try
        {
            Vinculacion.FechaDeInicioDeConvenio = DateTime.Parse(Request.Form["FechaInicio"]);
            Vinculacion.OrganizacionIid = Convert.ToInt32(Request.Form["orgId"]);
            bool existe = _context.Vinculacions.ToList().Any(v => v.OrganizacionIid.Equals(Vinculacion.OrganizacionIid));
            if (!existe)
            {
                _context.Vinculacions.Add(Vinculacion);
                _context.SaveChanges();
                if (fileVinculacion != null)
                {
                    string fecha = DateTime.Now.ToString().Replace("/", "");
                    string fileName = "VINCULACION_" + fecha.Replace(" ", "").Replace(":", "");
                    var archivo = Path.Combine(Environment.WebRootPath, "Archivos", fileName);
                    using (var fileStream = new FileStream(archivo, FileMode.Create))
                    {
                        await fileVinculacion.CopyToAsync(fileStream);
                    }

                    ArchivoVinculacion.NombreArchivo = fileName +"."+fileVinculacion.ContentType.Split("/")[1];
                    ArchivoVinculacion.IdFuente = Vinculacion.VinculacionId;
                    ArchivoVinculacion.Direccion = "Archivos/" + fileName;
                    ArchivoVinculacion.Fuente = "proyectos";
                    ArchivoVinculacion.TipoContenido = fileVinculacion.ContentType;
                    _context.Archivos.Add(ArchivoVinculacion);
                    _context.SaveChanges();
                }
                TempData["SuccessMessage"] = "Registro completado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "El proyecto vinculación ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Error al registrar el proyecto vinculación " + e.Message;
        }
        cargarOrganizaciones();
        return Page();
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