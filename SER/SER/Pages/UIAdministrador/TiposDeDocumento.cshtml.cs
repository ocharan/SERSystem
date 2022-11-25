using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Context;
using SER.Entities;

namespace SER.Pages.UIAdministrador;

public class TiposDeDocumento : PageModel
{
    private readonly MySERContext _context;
    
    public List<TipoDocumento> TiposDeDocumentos { get; set; }

    public TiposDeDocumento(MySERContext context)
    {
        _context = context;
        TiposDeDocumentos = new List<TipoDocumento>();
    }
    
    public void OnGet()
    {
        cargarTiposDocumento();
    }

    public void cargarTiposDocumento()
    {
        var listaTipos = _context.TipoDocumentos.ToList();
        foreach (var tipo in listaTipos)
        {
            TipoDocumento tipoRegistrado = new TipoDocumento()
            {
                NombreDocumento = tipo.NombreDocumento,
                ExperienciaEducativa = tipo.ExperienciaEducativa,
                IdTipo = tipo.IdTipo
            };
            TiposDeDocumentos.Add(tipoRegistrado);
        }
    }
}