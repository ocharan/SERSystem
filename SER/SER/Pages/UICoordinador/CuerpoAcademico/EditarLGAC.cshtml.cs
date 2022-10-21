using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarLGAC : PageModel
{

    private readonly MySERContext _context;
    public List<Entidades.CuerpoAcademico> CuerpoAcademicos { get; set; }
    
    
    [BindProperty]
    public Lgac lgacNuevo { get; set; }

    public EditarLGAC(MySERContext context)
    {
        _context = context;
        lgacNuevo = new Lgac();
        CuerpoAcademicos = new List<Entidades.CuerpoAcademico>();
    }
    public void OnGet()
    {
        try
        {
            getInfoLgac();
            getCuerposAcademicos();
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información " + ex.Message;
        }
    }

    public void OnPost()
    {
        var lgacExistentes = _context.Lgacs.ToList();
        var idlgac = Request.Query["id"];
        var lgac = _context.Lgacs.First(l => l.LgacId == Int32.Parse(idlgac));
        if (lgac.Nombre == lgacNuevo.Nombre)
        {
            lgac.Descripcion = lgacNuevo.Descripcion;
            lgac.CuerpoAcademicoId = lgacNuevo.CuerpoAcademicoId;
            _context.SaveChanges();
            TempData["Success"] = "LGAC actualizado correctamente";
        }
        else
        {
            bool existeLGAC = lgacExistentes.Any(l => l.Nombre.Equals(lgacNuevo.Nombre));
            if (!existeLGAC)
            {
                lgac.Nombre = lgacNuevo.Nombre;
                lgac.Descripcion = lgacNuevo.Descripcion;
                lgac.CuerpoAcademicoId = lgacNuevo.CuerpoAcademicoId;
                _context.SaveChanges();
                TempData["Success"] = "LGAC actualizado correctamente";
            }
            else
            {
                TempData["Error"] = "El nombre del LGAC a actualizar ya existe";
            }
        }
        
    }
    
    public void getInfoLgac()
    {
        var idLgac = Request.Query["id"];
        var legac = _context.Lgacs.FirstOrDefault(l => l.LgacId == Int32.Parse(idLgac));
        lgacNuevo.Nombre = legac.Nombre;
        lgacNuevo.Descripcion = legac.Descripcion;
        lgacNuevo.CuerpoAcademicoId = legac.CuerpoAcademicoId;
    }

    public void getCuerposAcademicos()
    {
        var cuerposRegistrados = _context.CuerpoAcademicos.ToList();
        foreach (var cuerpoAcademico in cuerposRegistrados)
        {
            Entidades.CuerpoAcademico cuerpo = new Entidades.CuerpoAcademico()
            {
                Nombre = cuerpoAcademico.Nombre,
                CuerpoAcademicoId = cuerpoAcademico.CuerpoAcademicoId
            };
            CuerpoAcademicos.Add(cuerpo);
        }
    }
}