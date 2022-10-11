using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;
using Microsoft.EntityFrameworkCore;

namespace SER.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegistrarDocumentoProyectoGuiadoModel : PageModel
    {
        /*private readonly MySERContext _context;
        public List<ExperienciaEducativa> EEProyectoGuiado { get; set; }
        public List<Alumno> Alumnos { get; set; }
        public List<AlumnoProyectoGuiado> AlumnosProyectoGuiado { get; set; }
        private IWebHostEnvironment Environment;

        [BindProperty]
        public string IDAlumnoSeleccionado { get; set; }
        [BindProperty]
        public Documento DocumentoProyectoGuiado { get; set; }
        [BindProperty]
        public IFormFile ArchivoDeProyectoGuiado { get; set; }
        [BindProperty]
        public string NombreDocumento { get; set; }
        [BindProperty]
        public string NotaDocumento { get; set; }
        public RegistrarDocumentoProyectoGuiadoModel(MySERContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
            AlumnosProyectoGuiado = new List<AlumnoProyectoGuiado>();
            Alumnos = new List<Alumno>();
        }
        public IActionResult OnGet()
        {
            try
            {
                getAlumnosProyectoGuiado();

                if (!AlumnosProyectoGuiado.Any())
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "No hay aulumnos registrados en la EE Proyectio Guiado");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudieron obtener los alumnos de la experiencia educativa de Proyecto Guiado.");
            }

            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            getAlumnosProyectoGuiado();
            AlumnoProyectoGuiado alumnoSeleccionado = AlumnosProyectoGuiado.Where(a => a.Matricula.Equals(IDAlumnoSeleccionado)).FirstOrDefault();
            DocumentoProyectoGuiado.TrabajoRecepcionalId = alumnoSeleccionado.TrabajoRecepcionalId;
            DocumentoProyectoGuiado.ExperienciaEducativaId = alumnoSeleccionado.ExperienciaEducativaID;
            if (ArchivoDeProyectoGuiado != null)
            {
                if (ArchivoDeProyectoGuiado.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ArchivoDeProyectoGuiado.CopyTo(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        DocumentoProyectoGuiado.Archivo = fileBytes;
                    }
                }

            }
            _context.Documentos.Add(DocumentoProyectoGuiado);
            _context.SaveChanges();
            return new OkObjectResult("Documento de proyecto guiado guardado.");
        }

        public void getAlumnosProyectoGuiado()
        {
            var alumnosProyectoGuiado = _context.AlumnoTrabajoRecepcionalProyectoGuiadoViews.ToList();

            foreach (var alumno in alumnosProyectoGuiado)
            {
                AlumnoProyectoGuiado alumnoPG = new AlumnoProyectoGuiado()
                {
                    Nombre = alumno.Nombre,
                    CorreoElectronico = alumno.CorreoElectronico,
                    Matricula = alumno.Matricula,
                    Modalidad = alumno.Modalidad,
                    Estado = alumno.Estado,
                    Fechadeinicio = alumno.ShortDateTime(alumno.Fechadeinicio),
                    ExperienciaEducativaID = alumno.ExperienciaEducativaId,
                    TrabajoRecepcionalId = alumno.TrabajoRecepcionalId
                };
                AlumnosProyectoGuiado.Add(alumnoPG);
            }
        }*/
    }
}
