using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting;
using SER.Entidades;
using SER.DBContext;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class RegistrarLgac : PageModel
{
    private readonly MySERContext _context;
    public List<Entidades.CuerpoAcademico> CuerpoAcademicos { get; set; }
    [BindProperty]
    public Lgac lgacNuevo { get; set; }
    
    public RegistrarLgac(MySERContext context)
    {
        _context = context;
        CuerpoAcademicos = new List<Entidades.CuerpoAcademico>();
    }
    
    public void OnGet()
    {
        try
        {
            getCuerposAcademicos();
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ha ocurrido un error al cargar la información";
        }
    }

    [HttpPost]
    public void OnPost()
    {
        if (ModelState.IsValid)
        {
            var lgacExistentes = _context.Lgacs.ToList();
            bool existeLGACT = lgacExistentes.Any(l => l.Nombre.Equals(lgacNuevo.Nombre));
            if (!existeLGACT)
            {
                _context.Lgacs.Add(lgacNuevo);
                _context.SaveChanges();
                TempData["Success"] = "LGAC registrado correctamente";
            }
            else
            {
                TempData["Error"] = "El LGAC que intenta registrar ya existe";
            }

        }
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
