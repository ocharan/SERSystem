using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;
[Authorize(Roles = "Administrador")]
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

    public void OnPost()
    {
        try
        {
            var listaDocentes = _context.Profesors.ToList();
            profesorNuevo.Nombre = Request.Form["nombreProfesor"] + " " + Request.Form["apellidoPaterno"] + " " +
                                   Request.Form["apellidoMaterno"];
            profesorNuevo.Nombre = profesorNuevo.Nombre.ToUpper();
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
                    TempData["Success"] = "Docente registrado correctámente";
                }
                else
                {
                    TempData["Error"] = "El docente que inténtas registrar ya existe";
                }
            }
            else
            {
                TempData["Error"] = "Las contraseñas deben ser iguales";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error durante el registro " + e.Message;
        }
    }


}