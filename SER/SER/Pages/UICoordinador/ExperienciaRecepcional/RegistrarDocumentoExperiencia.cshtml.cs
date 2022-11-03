using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages.UICoordinador.ExperienciaRecepcional;

public class RegistrarDocumentoExperiencia : PageModel
{

    private readonly MySERContext _context;
    
    public List<TipoDocumento> TipoDocumentos { get; set; }

    public RegistrarDocumentoExperiencia(MySERContext context)
    {
        _context = context;
        TipoDocumentos = new List<TipoDocumento>();
    }

    public void OnGet()
    {
        getTiposDocumento();
    }

    public void getTiposDocumento()
    {
        var listaTipos = _context.TipoDocumentos.ToList().Where(t=> t.ExperienciaEducativa=="Experiencia Recepcional");
        foreach (var tipo in listaTipos)
        {
            TipoDocumento tipoDocumento = new TipoDocumento()
            {
                NombreDocumento = tipo.NombreDocumento
            };
            TipoDocumentos.Add(tipoDocumento);
        }
    }
}

