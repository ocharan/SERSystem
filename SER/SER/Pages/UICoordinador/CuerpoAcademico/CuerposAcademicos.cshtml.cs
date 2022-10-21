using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.CuerpoAcademico;

public class CuerposAcademicos : PageModel
{

    private readonly MySERContext _context;

    [BindProperty] public string idAcademico { get; set; }
    public List<Entidades.CuerpoAcademico> CuerpoAcademicos { get; set; }
    
    //Paginación
    
    public int CurrentPage { get; set; } = 1;

    public int Count { get; set; }

    public int PageSize { get; set; } = 5;

    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

    public bool EnablePrevious => CurrentPage > 1;

    public bool EnableNext => CurrentPage < TotalPages;

    public bool isSearch { get; set; }
    
    public CuerposAcademicos(MySERContext context)
    {
        _context = context;
        CuerpoAcademicos = new List<Entidades.CuerpoAcademico>();

    }

    public void OnGet(int currentPage)
    {
        if (!isSearch)
        {
            getCuerposAcademicos();
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            Count = CuerpoAcademicos.Count;
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            CuerpoAcademicos = CuerpoAcademicos.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
        else
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            Count = CuerpoAcademicos.Count;
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            CuerpoAcademicos = CuerpoAcademicos.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
        
       
    }

    public IActionResult OnPostModificar()
    {
        var id = Request.Query["id"];
        return Redirect("EditarCuerpoAcademico?id="+id);
    }
    
    public void OnPostBuscar()
    {
        Console.WriteLine("Buscar");
        isSearch = true;
        var query = Request.Form["query"];
        try
        {
            IQueryable<Entidades.CuerpoAcademico> resultadoBusqueda;
            if (query == "" )
            {
                resultadoBusqueda = _context.CuerpoAcademicos;
            }
            else
            {
                resultadoBusqueda = _context.CuerpoAcademicos.Where(a => a.Nombre.Contains(query));
            }
            if (resultadoBusqueda.Count() > 0)
            {
                CuerpoAcademicos.Clear();
                foreach (var cuerpoCA in resultadoBusqueda)
                {
                    Entidades.CuerpoAcademico cuerpo = new Entidades.CuerpoAcademico()
                    {
                        Nombre = cuerpoCA.Nombre,
                        CuerpoAcademicoId = cuerpoCA.CuerpoAcademicoId,
                    };
                    CuerpoAcademicos.Add(cuerpo);
                }
                Count = CuerpoAcademicos.Count;
                if (CurrentPage > TotalPages)
                    CurrentPage = TotalPages;
                CuerpoAcademicos = CuerpoAcademicos.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                TempData["Error"] = "No se encontraron resultados relacionados con tú búsqueda";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Error al establecer conexión";
        }
    }


    public IActionResult OnPostRegistrar()
    {
        var id = Request.Query["id"];
        return Redirect("RegistrarIntegrantesCA?id="+id);
    }
    
    public void getCuerposAcademicos()
    {
        isSearch = false;
        try
        {
            var listaCuerpos = _context.CuerpoAcademicos.ToList();
            foreach (var cuerpoCA in listaCuerpos)
            {
                Entidades.CuerpoAcademico cuerpo = new Entidades.CuerpoAcademico()
                {
                    Nombre = cuerpoCA.Nombre,
                    CuerpoAcademicoId = cuerpoCA.CuerpoAcademicoId,
                };
                CuerpoAcademicos.Add(cuerpo);
            }
        }
        catch (Exception e)
        {
            TempData["ExceptionMessage"] = "Error al cargar los cuerpos academicos";
        }
    }
    
  
}