using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using SER.Context;
using SER.DTO;
using SER.Entities;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class ModificarTrabajoRecepcional : PageModel
{
    private readonly MySERContext _context;

    [BindProperty] public TrabajoRecepcional TrabajoRecepcional { get; set; }
    
    [BindProperty] public string tipoTrabajo { get; set; }
    
    [BindProperty] public Archivo ArchivoTrabajo { get; set; }
    
    [BindProperty] public string estado { get; set; }
    public string proyectoAsociado { get; set; }
    
    public List<Pladeafei> Pladeafeis { get; set; }

    public List<VinculacionOrg> Vinculacions { get; set; }
    
    public List<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }

    private IWebHostEnvironment _environment;

    private bool tieneDocumento = false;

    public ModificarTrabajoRecepcional(MySERContext context, IWebHostEnvironment environment)
    {
        _environment = environment;
        _context = context;
        TrabajoRecepcional = new TrabajoRecepcional();
        Pladeafeis = new List<Pladeafei>();
        Vinculacions = new List<VinculacionOrg>();
        ProyectoDeInvestigacions = new List<ProyectoDeInvestigacion>();
    }
    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { succes = true });
    }

    public async Task<IActionResult> OnPostModificarTrabajo(IFormFile fileTrabajo)
    {
        try
        {
            var trabajoModificar = _context.TrabajoRecepcionals.FirstOrDefault(t =>
                t.TrabajoRecepcionalId.Equals(Int32.Parse(Request.Query["id"])));
            trabajoModificar.Nombre = Request.Form["NombreTrabajo"];
            trabajoModificar.Duracion = Request.Form["DuracionTrabajo"];
            trabajoModificar.Fechadeinicio = DateTime.Parse(Request.Form["FechaTrabajo"]);
            trabajoModificar.LineaDeInvestigacion = Request.Form["LineaTrabajo"];
            trabajoModificar.Modalidad = Request.Form["ModalidadTrabajo"];
            trabajoModificar.Estado = Request.Form["estadoTrabajo"];
            trabajoModificar.ExperienciaActual = Request.Form["experienciaEstado"];
            var tipoTrabajo = Request.Form["TipoTrabajo"];
            if (tipoTrabajo == "vinculacion")
            {
                trabajoModificar.VinculacionId = Int32.Parse(Request.Form["idProyecto"]);
            }else if (tipoTrabajo == "pladea")
            {
                trabajoModificar.PladeafeiId = Int32.Parse(Request.Form["idProyecto"]);
            }
            else
            {
                trabajoModificar.ProyectoDeInvestigacionId = Int32.Parse(Request.Form["idProyecto"]);
            }

            if (fileTrabajo != null)
            {
                string fecha = DateTime.Now.ToString().Replace("/", "");
                string fileName = "Anteproyecto_"+trabajoModificar.TrabajoRecepcionalId+ fecha.Replace(" ", "").Replace(":", "");
                var archivo = Path.Combine(_environment.WebRootPath, "Archivos", fileName);
                borrararchivoTrabajo(trabajoModificar.TrabajoRecepcionalId);
                using (var fileStream = new FileStream(archivo, FileMode.Create))
                {
                    await fileTrabajo.CopyToAsync(fileStream);
                }
                if (!tieneDocumento)
                {
                    var tipo =
                        _context.TipoDocumentos.FirstOrDefault(t => t.NombreDocumento.Equals("Anteproyecto"));
                    Documento documento = new Documento();
                    documento.TrabajoRecepcionalId = trabajoModificar.TrabajoRecepcionalId;
                    if (tipo == null)
                    {
                        TipoDocumento tipoDocumento = new TipoDocumento()
                        {
                            NombreDocumento = "Anteproyecto",
                            ExperienciaEducativa = "Experiencia Recepcional"
                        };
                        _context.TipoDocumentos.Add(tipoDocumento);
                        _context.SaveChanges();
                        documento.TipoDocumentoId = tipoDocumento.IdTipo;
                        _context.Documentos.Add(documento);
                        _context.SaveChanges();
                    }
                    else
                    {
                        documento.TipoDocumentoId = tipo.IdTipo;
                        _context.Documentos.Add(documento);
                        _context.SaveChanges();
                    }
                }
                ArchivoTrabajo = new Archivo();
                ArchivoTrabajo.NombreArchivo = fileName +"."+fileTrabajo.ContentType.Split("/")[1];
                ArchivoTrabajo.IdFuente = trabajoModificar.TrabajoRecepcionalId;
                ArchivoTrabajo.Direccion = "Archivos/" + fileName;
                ArchivoTrabajo.Fuente = "Anteproyecto";
                ArchivoTrabajo.TipoContenido = fileTrabajo.ContentType;
                _context.Archivos.Add(ArchivoTrabajo);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Trabajo recepcional actualizado correctamente";
            }
            else
            {
                TempData["SuccessMessage"] = "Trabajo recepcional actualizado correctamente";
                _context.SaveChanges();
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar la información, "+e.Message;
        }
        cargarInformacionTrabajo();
        return Page();
    }

    public void borrararchivoTrabajo(int idTrabajo)
    {
        var archivo = _context.Archivos.FirstOrDefault(a => a.IdFuente == idTrabajo && a.Fuente.Equals("Anteproyecto"));
        if (archivo!=null)
        {
            System.IO.File.Delete(_environment.WebRootPath+"/"+archivo.Direccion);
            _context.Remove(archivo);
            _context.SaveChanges();
        }
    }
    public void OnGet()
    {
        try
        {
            cargarInformacionTrabajo();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Ha ocurrido un error al cargar la información del trabajo, "+e.Message;
        }
    }

    public void cargarInformacionTrabajo()
    {
        var trabajo =
            _context.TrabajoRecepcionals.FirstOrDefault(t =>
                t.TrabajoRecepcionalId == Int32.Parse(Request.Query["id"]));
        var archivoTrabajo = _context.Archivos.FirstOrDefault(a => a.IdFuente.Equals(Int32.Parse(Request.Query["id"])) && a.Fuente.Equals("Anteproyecto"));
        estado = trabajo.Estado;
        TrabajoRecepcional.Nombre = trabajo.Nombre;
        TrabajoRecepcional.Duracion = trabajo.Duracion;
        TrabajoRecepcional.Fechadeinicio = trabajo.Fechadeinicio;
        TrabajoRecepcional.LineaDeInvestigacion = trabajo.LineaDeInvestigacion;
        TrabajoRecepcional.Modalidad = trabajo.Modalidad;
        TrabajoRecepcional.ExperienciaActual = trabajo.ExperienciaActual;
        if (trabajo.PladeafeiId > -1)
        {
            proyectoAsociado = "pladea";
            cargarPladeas();
            TrabajoRecepcional.PladeafeiId = trabajo.PladeafeiId;
        }else if (trabajo.VinculacionId > -1)
        {
            proyectoAsociado = "vinculacion";
            cargarVinculacion();
            TrabajoRecepcional.VinculacionId = trabajo.VinculacionId;
        }else if (trabajo.ProyectoDeInvestigacionId > -1)
        {
            proyectoAsociado = "investigacion";
            cargarInvestigacion();
            TrabajoRecepcional.ProyectoDeInvestigacionId = trabajo.ProyectoDeInvestigacionId;
        }

        if (archivoTrabajo !=null)
        {
            tieneDocumento = true;
            ArchivoTrabajo = new Archivo();
            ArchivoTrabajo.NombreArchivo = archivoTrabajo.NombreArchivo;
            ArchivoTrabajo.Direccion = archivoTrabajo.Direccion;
        }
    }

    public void cargarPladeas()
    {
        Pladeafeis = _context.Pladeafeis.ToList();
    }

    public void cargarVinculacion()
    {
        var listaVinculacion = _context.Vinculacions.ToList();
        var listaOrgs = _context.Organizacions.ToList();        
        var vinculaciones = listaVinculacion.Join(listaOrgs, vinculacion => vinculacion.OrganizacionIid,
            organizacion => organizacion.OrganizacionId, (vinculacion, organizacion) => new
            {
                nombre = organizacion.Nombre,
                id = vinculacion.VinculacionId
            });
        foreach (var vinculacion in vinculaciones)
        {
            VinculacionOrg vinculacionOrg = new VinculacionOrg()
            {
                organizacion = vinculacion.nombre,
                vinculacionId = vinculacion.id.ToString()
            };
            Vinculacions.Add(vinculacionOrg);
        }
    }

    public void cargarInvestigacion()
    {
        ProyectoDeInvestigacions = _context.ProyectoDeInvestigacions.ToList();
    }
    
    public JsonResult OnGetObtenerPladeaModificar()
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

    public JsonResult OnGetObtenerInvestigacionModificar()
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

    public JsonResult OnGetObtenerVinculacionModificar()
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
    
    public IActionResult OnPostFile()
    {
        var id = Request.Query["id"];
        var fileTrabajo = _context.Archivos.FirstOrDefault(a => a.IdFuente == Int32.Parse(id));
        return File(fileTrabajo.Direccion, fileTrabajo.TipoContenido);
    }
}