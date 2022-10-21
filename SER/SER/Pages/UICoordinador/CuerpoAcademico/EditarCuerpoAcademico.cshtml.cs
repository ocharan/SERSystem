using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Remotion.Linq.Clauses;
using SER.DBContext;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class EditarCuerpoAcademico : PageModel
{
    public string idCuerpo { get; set; }

    public readonly MySERContext _Context;

    [BindProperty]
    public Entidades.CuerpoAcademico _cuerpoAcademico { get; set; }
    public EditarCuerpoAcademico(MySERContext context)
    {
        _Context = context;
        _cuerpoAcademico = new Entidades.CuerpoAcademico();
    }
    public void OnGet()
    {
        idCuerpo = Request.Query["id"];
        getCuerpo();
    }

    public void OnPost()
    {
        try
        {
            var cuerposExistentes = _Context.CuerpoAcademicos.ToList();
            idCuerpo = Request.Query["id"];
            var cuerpo = _Context.CuerpoAcademicos.First(c => c.CuerpoAcademicoId == Int32.Parse(idCuerpo));
            bool existeCuerpo = cuerposExistentes.Any(c => c.Nombre.Equals(_cuerpoAcademico.Nombre));
            if (cuerpo.Nombre == _cuerpoAcademico.Nombre)
            {
                cuerpo.Objetivogeneral = _cuerpoAcademico.Objetivogeneral;
                _Context.SaveChanges();
                TempData["SuccessMessage"] = "Cuerpo academico actualizado correctamente";
            }
            else
            {
                if (!existeCuerpo)
                {
                    cuerpo.Nombre = _cuerpoAcademico.Nombre;
                    cuerpo.Objetivogeneral = _cuerpoAcademico.Objetivogeneral;
                    _Context.SaveChanges();
                    TempData["SuccessMessage"] = "Cuerpo academico actualizado correctamente";
                }else
                {
                    TempData["ErrorMessage"] = "El nombre del cuerpo a ingresar ya existe";
                }
            }
            
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = e.Data;
        }
    }
    

    public void getCuerpo()
    {
        var cuerpo = _Context.CuerpoAcademicos.FirstOrDefault(c => c.CuerpoAcademicoId == Int64.Parse(idCuerpo));
        _cuerpoAcademico.Nombre = cuerpo.Nombre;
        _cuerpoAcademico.Objetivogeneral = cuerpo.Objetivogeneral;
    }
}