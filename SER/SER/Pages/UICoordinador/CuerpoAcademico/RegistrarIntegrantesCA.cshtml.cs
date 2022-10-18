using System.Text.Json.Nodes;
using Arch.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using NuGet.Protocol;
using SER.DBContext;
using SER.DTO;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistrarIntegrantesCA : PageModel
{
    private readonly MySERContext _context;
    public List<ProfesorIntegrante> Profesors { get; set; }
    
    public List<Profesor> profesores { get; set; }

    [BindProperty]
    public Integrante integranteNuevo { get; set; }
    
    public string idAcademiaAsignar { get; set; }
    
    public List<Integrante> Integrantes { get; set; }


    public RegistrarIntegrantesCA(MySERContext context)
    {
        _context = context;
        Profesors = new List<ProfesorIntegrante>();
        profesores = new List<Profesor>();
        Integrantes = new List<Integrante>();
    }
    
    public void OnGet()
    {
        idAcademiaAsignar = Request.Query["id"];
        getIntegrantes();
    }

    public JsonResult OnGetObtenerDocentes(string idAcademiaA)
    {
        getProfesores(idAcademiaA);
        return new JsonResult(Profesors.ToJson());
    }
    
    public void getProfesores(string idAcademia)
    {
        var listaIntegrantes = _context.Integrantes
            .Where(i => i.CuerpoAcademicoId == Int32.Parse(idAcademia)).ToList();
        var listaProfesoresIntegrantes = _context.Integrantes.ToList();
        var listaProfesores = _context.Profesors.ToList();
        foreach (var profesor in listaProfesores)
        {
            var existe = listaIntegrantes.Any(i => i.NumeroDePersonal.Equals(profesor.NumeroDePersonal));
            if (!existe)
            {
                var yaEsIntegrante = listaProfesoresIntegrantes.Any(d =>
                    d.NumeroDePersonal == profesor.NumeroDePersonal && d.Tipo == "Integrante");
                ProfesorIntegrante profesorNuevo = new ProfesorIntegrante()
                {
                    NumeroDePersonal = profesor.NumeroDePersonal,
                    Nombre = profesor.Nombre,
                    ProfesorId = profesor.ProfesorId,
                    isIntegrante = yaEsIntegrante
                };
                Profesors.Add(profesorNuevo);
            }
        }
    }
    
    public void getIntegrantes()
    {
        var listaIntegrantes = _context.Integrantes.ToList();
        foreach (var integrante in listaIntegrantes)
        {
            if (integrante.CuerpoAcademicoId.Equals(Int32.Parse(idAcademiaAsignar)))
            {
                Integrante integranteObtenido = new Integrante()
                {
                    Nombre = integrante.Nombre,
                    Tipo = integrante.Tipo,
                    IntegranteId = integrante.IntegranteId,
                    CuerpoAcademicoId = integrante.CuerpoAcademicoId,
                    NumeroDePersonal = integrante.NumeroDePersonal
                };
                Integrantes.Add(integranteObtenido);
            }
        }
    }

    public void OnPostEliminarIntegrante(int idEliminar)
    {
        var existe = _context.Integrantes.SingleOrDefault(x => x.IntegranteId == idEliminar);
        if (existe != null)
        {
            _context.Integrantes.Remove(existe);
            _context.SaveChanges();
        }
    }
    
    [HttpPost]
    public IActionResult OnPostGuardarRegistro(string NumeroDePersonal, string Nombre, string ProfesorId, string Tipo, string cuerpoID)
    {
        Integrante integranteNuevo = new Integrante();
        integranteNuevo.Nombre = Nombre;
        integranteNuevo.NumeroDePersonal = NumeroDePersonal;
        integranteNuevo.Tipo = Tipo;
        integranteNuevo.CuerpoAcademicoId = Int32.Parse(cuerpoID);
        _context.Integrantes.Add(integranteNuevo);
        _context.SaveChanges();
        return Page();
    }

    public Entidades.CuerpoAcademico obtenerCuerpo(int idCuerpo, List<Entidades.CuerpoAcademico> listaCuerpos)
    {
        foreach (var cuerpo in listaCuerpos)
        {
            if (cuerpo.CuerpoAcademicoId == idCuerpo)
            {
                return cuerpo;
            }
        }
        return null;
    }
    
}