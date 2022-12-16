using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.DTO;
using SER.Entities;

namespace SER.Pages.UIMaestro;
[Authorize(Roles = "Maestro")]
public class VerExpediente : PageModel
{
    private readonly MySERContext Context;
    private IWebHostEnvironment Environment;

    public List<SinodalAsignado> sinodalesAsignados { get; set; }
    public List<DocumentoVista> documentosRegistrados { get; set; }
    
    [BindProperty] public Expediente expediente { get; set; }

    public VerExpediente(MySERContext _context, IWebHostEnvironment _environment)
    {
        Context = _context;
        Environment = _environment;
        sinodalesAsignados = new List<SinodalAsignado>();
        documentosRegistrados = new List<DocumentoVista>();
        expediente = new Expediente();
    }
    
    public void OnGet()
    {
        cargarExpediente();
        cargarDocumentos();
        cargarSinodales();
    }


    public void cargarExpediente()
    {
        var exp = Context.Expedientes.FirstOrDefault(e => e.Matricula.Equals(Request.Query["id"]));

        expediente.Matricula = exp.Matricula;
        expediente.CorreoElectronico = exp.CorreoElectronico;
        expediente.NombreAlumno = exp.NombreAlumno;
        expediente.Modalidad = exp.Modalidad;
        expediente.Estado = exp.Estado;
        expediente.LineaDeInvestigacion = exp.LineaDeInvestigacion;
        expediente.Nombre = exp.Nombre;
        expediente.TrabajoRecepcionalId = exp.TrabajoRecepcionalId;
    }

    public void cargarSinodales()
    {
        var listaSinodales = Context.SinodalDelTrabajos.ToList();
        var listaSinodalesTrabajo = Context.TrabajoRecepcionalSinodalDelTrabajos.ToList();
        var listaSinodalesAsignados = listaSinodalesTrabajo.Join(listaSinodales,
            sinodalAsignado => sinodalAsignado.SinodalDelTrabajoId,
            sinodal => sinodal.SinodalDelTrabajoId, ((sinodalTrabajo, sinodalAsignado) => new
            {
                Nombre = sinodalAsignado.Nombre,
                Tipo = sinodalTrabajo.TipoSinodal,
                idTrabajo = sinodalTrabajo.TrabajoRecepcionalId,
                idSinodal = sinodalAsignado.SinodalDelTrabajoId
            }));
        foreach (var sinodalAsignado in listaSinodalesAsignados)
        {
            if (sinodalAsignado.idTrabajo == expediente.TrabajoRecepcionalId)
            {
                SinodalAsignado sinodal = new SinodalAsignado()
                {
                    nombre = sinodalAsignado.Nombre,
                    tipo = sinodalAsignado.Tipo,
                    trabajoId = sinodalAsignado.idTrabajo,
                    idSinodal = sinodalAsignado.idSinodal
                };
                sinodalesAsignados.Add(sinodal);
            }
        }
    }
    
    public void cargarDocumentos()
    {
        var documentos = Context.Documentos.Where(d => d.TrabajoRecepcionalId == expediente.TrabajoRecepcionalId)
            .ToList();
        var tiposDocumento = Context.TipoDocumentos.ToList();

        var listaDocumentos = documentos.Join(tiposDocumento, documento => documento.TipoDocumentoId,
            tipo => tipo.IdTipo, ((documento, tipo) => new
            {
                nombreTipo = tipo.NombreDocumento,
                idDocumento = documento.DocumentoId,
                idTrabajo = documento.TrabajoRecepcionalId
            }));

        foreach (var doc in listaDocumentos)
        {
            DocumentoVista documentoVista = new DocumentoVista()
            {
                nombreTipo = doc.nombreTipo,
                idDocumento = doc.idDocumento,
                idTrabajo = (int)doc.idTrabajo
            };
            documentosRegistrados.Add(documentoVista);
        }

    }
    
    [HttpPost]
    public async Task<IActionResult> OnPostCerrarSesion()
    {
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { success = true });
    }

    public IActionResult OnPostRegistrarDocumento()
    {
        var trabajo =
            Context.TrabajoRecepcionals.FirstOrDefault(t => t.TrabajoRecepcionalId == Int32.Parse(Request.Query["id"]));
        if (trabajo.ExperienciaActual.Equals("pg"))
        {
            return Redirect("RegistrarDocumentoProyectoGuiado?id=" + Request.Query["id"]);
        }
        else
        {
            return Redirect("RegistrarDocumentoExperiencia?id=" + Request.Query["id"]);
        }
    }

    public IActionResult OnPostVerDocumento()
    {
        var tipo = Request.Query["tipo"].ToString().Replace("%", " ");
        var archivo = Context.Archivos
            .FirstOrDefault(a => a.IdFuente == Int32.Parse(Request.Query["id"]) && a.Fuente.Equals(tipo));
        return File(archivo.Direccion, archivo.TipoContenido);
    }
}