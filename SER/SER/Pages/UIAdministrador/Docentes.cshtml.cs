using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
public class Docentes : PageModel
{

    private readonly MySERContext _context;
    public List<Entities.Profesor> Profesors { get; set; }

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