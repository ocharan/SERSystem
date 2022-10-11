using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UIAdministrador;

public class RegistrarDocente : PageModel
{
    private readonly MySERContext _context;
    
    [BindProperty] 
    public Profesor profesorNuevo { get; set; }
    [BindProperty]
    public Usuario usuarioNuevo { get; set; }

    [BindProperty]
    public string contraseña { get; set; }


    public RegistrarDocente(MySERContext context)
    {
        _context = context;
    }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        var listaDocentes = _context.Profesors.ToList();
        bool existeDocente = listaDocentes.Any(d => d.NumeroDePersonal.Equals(profesorNuevo.NumeroDePersonal));
        if (contraseña.Equals(usuarioNuevo.Contra))
        {
            if (!existeDocente)
            {
                usuarioNuevo.NombreUsuario = profesorNuevo.NumeroDePersonal;
                usuarioNuevo.Tipo = "Maestro";
                _context.Profesors.Add(profesorNuevo);
                _context.Usuarios.Add(usuarioNuevo);
                _context.SaveChanges();
                return RedirectToPage("Docentes");
            }
            else
            {
                return Page();
            }
        }
        else
        {
            return Page();
        }
        return Page();
    }


}