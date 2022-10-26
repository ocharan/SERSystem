using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UIAdministrador;

public class Docentes : PageModel
{

    private readonly MySERContext _context;
    public List<Entidades.Profesor> Profesors { get; set; }

    public Docentes(MySERContext context)
    {
        _context = context;
        Profesors = new List<Profesor>();
    }
    public void OnGet()
    {
        try
        {
            getDocentes();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public IActionResult OnPostModificar()
    {
        return Redirect("EditarDocente?id=" + Request.Query["id"]);
    }


    public void getDocentes()
    {
        var listaProfesores = _context.Profesors.ToList();
        foreach (var profesor in listaProfesores)
        {
            Profesor docente = new Profesor()
            {
                ProfesorId = profesor.ProfesorId,
                Nombre = profesor.Nombre,
                NumeroDePersonal = profesor.NumeroDePersonal
            };
            Profesors.Add(docente);
        }
    }
}