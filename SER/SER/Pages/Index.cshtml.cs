﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SER.DBContext;
using SER.Entidades;
using SER.Pages.Shared;

namespace SER.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MySERContext _context;
        
        [BindProperty]
        public Usuario Usuario { set; get; }
        public IndexModel(MySERContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> OnPost()
        {
            var usuarios = _context.Usuarios.ToList();
            var usuarioObtenido = usuarios.FirstOrDefault(usr => usr.NombreUsuario == Usuario.NombreUsuario && usr.Contra == Usuario.Contra);
            Console.WriteLine(Usuario.NombreUsuario);
            if (Usuario.NombreUsuario != "" || Usuario.Contra != "")
            {
                if (usuarioObtenido!=null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioObtenido.NombreUsuario)
                    };
                    claims.Add(new Claim(ClaimTypes.Role, usuarioObtenido.Tipo));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    if (usuarioObtenido.Tipo.Equals("Coordinador"))
                    {
                        return RedirectToPage("/Menus/UICoordinador");
                    }else if (usuarioObtenido.Tipo.Equals("Administrador"))
                    {
                        return RedirectToPage("/Menus/UIAdministración");
                    }
                }
                else
                {
                    TempData["Error"] = "Credenciales incorrectas";
                    return Page();
                }
            }
            else
            {
                TempData["Error"] = "Debe ingresar el usuario y contraseña";
                return Page();
            }
            return Page();
        }
    }
}