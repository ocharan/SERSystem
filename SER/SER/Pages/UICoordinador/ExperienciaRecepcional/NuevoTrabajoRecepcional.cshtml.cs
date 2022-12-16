using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

[Authorize(Roles = "Coordinador")]
public class NuevoTrabajoRecepcional : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty]
    public TrabajoRecepcional TrabajoRecepcional { get; set; }
    
    [BindProperty]
    public string proyectoAsociado { get; set; }
    public List<Pladeafei> Pladeafeis { get; set; }
    
    public List<Vinculacion> Vinculacions { get; set; }
    
    public List<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }

    private IWebHostEnvironment _environment;
    
    public Archivo archivoTrabajo { get; set; }
    
    public Documento Documento { get; set; }
    
    public NuevoTrabajoRecepcional(MySERContext context, IWebHostEnvironment environment)
    {
        _context = context;
        Pladeafeis = new List<Pladeafei>();
        Vinculacions = new List<Vinculacion>();
        ProyectoDeInvestigacions = new List<ProyectoDeInvestigacion>();
        _environment = environment;
        archivoTrabajo = new Archivo();
    }
    
    public void OnGet()
    {
    }

    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }
    public async Task<IActionResult> OnPost(IFormFile fileTrabajoRecepcional)
    {
        if (proyectoAsociado == "vinculacion")
        {
            TrabajoRecepcional.VinculacionId = (int?)Int64.Parse(Request.Form["idProyecto"]);
        }else if (proyectoAsociado == "pladea")
        {
            TrabajoRecepcional.PladeafeiId = (int?)Int64.Parse(Request.Form["idProyecto"]);
        }
        else
        {
            TrabajoRecepcional.ProyectoDeInvestigacionId = (int?)Int64.Parse(Request.Form["idProyecto"]);
        }
        try
        {
            TrabajoRecepcional.Estado = "En proceso";
            TrabajoRecepcional.ExperienciaActual = Request.Form["experienciaEstado"];
            bool existe = _context.TrabajoRecepcionals.Any(t => t.Nombre == TrabajoRecepcional.Nombre);
            if (!existe)
            {
                _context.TrabajoRecepcionals.Add(TrabajoRecepcional);
                _context.SaveChanges();
                if (fileTrabajoRecepcional != null)
                {
                    string fecha = DateTime.Now.ToString().Replace("/", "");
                    string fileName = "Anteproyecto_"+TrabajoRecepcional.TrabajoRecepcionalId+ fecha.Replace(" ", "").Replace(":", "");
                    var archivo = Path.Combine(_environment.WebRootPath, "Archivos", fileName);
                    using (var fileStream = new FileStream(archivo, FileMode.Create))
                    {
                        await fileTrabajoRecepcional.CopyToAsync(fileStream);
                    }

                    Documento = new Documento();
                    Documento.TrabajoRecepcionalId = TrabajoRecepcional.TrabajoRecepcionalId;
                    var Tipo = _context.TipoDocumentos.FirstOrDefault(t => t.NombreDocumento.Equals("Anteproyecto"));
                    if (Tipo != null)
                    {
                        Documento.TipoDocumentoId = Tipo.IdTipo;
                        _context.Documentos.Add(Documento);
                        _context.SaveChanges();
                    }
                    else
                    {
                        TipoDocumento tipoDocumento = new TipoDocumento()
                        {
                            NombreDocumento = "Anteproyecto",
                            ExperienciaEducativa = "Experiencia Recepcional"
                        };
                        _context.TipoDocumentos.Add(tipoDocumento);
                        _context.SaveChanges();
                        Documento.TipoDocumentoId = tipoDocumento.IdTipo;
                        _context.Add(Documento);
                        _context.SaveChanges();
                    }
                    archivoTrabajo.NombreArchivo = fileName +"."+fileTrabajoRecepcional.ContentType.Split("/")[1];
                    archivoTrabajo.IdFuente = TrabajoRecepcional.TrabajoRecepcionalId;
                    archivoTrabajo.Direccion = "Archivos/" + fileName;
                    archivoTrabajo.Fuente = "Anteproyecto";
                    archivoTrabajo.TipoContenido = fileTrabajoRecepcional.ContentType;
                    _context.Archivos.Add(archivoTrabajo);
                    _context.SaveChanges();
                    TempData["Success"] = "Trabajo recepcional registrado correctamente";
                }
                TempData["Success"] = "Trabajo recepcional registrado correctamente";
            }
            else
            {
                TempData["Error"] = "El trabajo recepcional que intentas registrar ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error durante el registro " + e.Message;
        }
        return Page();
    }

    public JsonResult OnGetObtenerPladea()
    {
        var listaPladea = _context.Pladeafeis.ToList();
        foreach (var pladea in listaPladea)
        {
            Pladeafei pladeafei = new Pladeafei()
            {
                Accion = pladea.Accion,
                PladeafeiId = pladea.PladeafeiId
            };
            Pladeafeis.Add(pladeafei);
        }

        return new JsonResult(Pladeafeis.ToJson());
    }

    public JsonResult OnGetObtenerInvestigacion()
    {
        var listaInvestigacion = _context.ProyectoDeInvestigacions.ToList();
        foreach (var investigacion in listaInvestigacion)
        {
            ProyectoDeInvestigacion proyectoDeInvestigacion = new ProyectoDeInvestigacion()
            {
                Nombre = investigacion.Nombre,
                ProyectoDeInvestigacionId = investigacion.ProyectoDeInvestigacionId
            };
            ProyectoDeInvestigacions.Add(proyectoDeInvestigacion);
        }
        return new JsonResult(ProyectoDeInvestigacions.ToJson());
    }

    public JsonResult OnGetObtenerVinculacion()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaOrgs = _context.Organizacions.ToList();
        var vinculaciones = listaVinculacion.Join(listaOrgs, vinculacion => vinculacion.OrganizacionIid,
            organizacion => organizacion.OrganizacionId, (vinculacion, organizacion) => new
            {
                nombre = organizacion.Nombre,
                id = vinculacion.VinculacionId
            });
        return new JsonResult(vinculaciones.ToJson());
    }
}