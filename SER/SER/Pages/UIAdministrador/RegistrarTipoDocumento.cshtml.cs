using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UIAdministrador;

public class RegistrarTipoDocumento : PageModel
{

    private readonly MySERContext _context;
    
    [BindProperty]
    public TipoDocumento TipoDocumento { get; set; }

    public RegistrarTipoDocumento(MySERContext context)
    {
        _context = context;
        TipoDocumento = new TipoDocumento();
    }
    
    public void OnGet()
    {
        
    }

    public void OnPost()
    {
        try
        {
            bool existe = _context.TipoDocumentos.Any(t => t.NombreDocumento == TipoDocumento.NombreDocumento);
            if (!existe)
            {
                _context.TipoDocumentos.Add(TipoDocumento);
                _context.SaveChanges();
                TempData["Success"] = "Tipo de documento registrado correctamente";
            }
            else
            {
                TempData["Error"] = "El tipo de documento que intentas registrar ya existe";
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "Ha ocurrido un error durante el registro "+e.Message;
        }
    }
}