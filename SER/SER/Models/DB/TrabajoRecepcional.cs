using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class TrabajoRecepcional
    {
        public TrabajoRecepcional()
        {
            AlumnoTrabajoRecepcionals = new HashSet<AlumnoTrabajoRecepcional>();
            Documentos = new HashSet<Documento>();
            ExamenDefensas = new HashSet<ExamenDefensa>();
            TrabajoRecepcionalSinodalDelTrabajos = new HashSet<TrabajoRecepcionalSinodalDelTrabajo>();
            Integrantes = new HashSet<Integrante>();
        }

        public int TrabajoRecepcionalId { get; set; }
        public string? Estado { get; set; }
        public DateTime? Fechadeinicio { get; set; }
        public string? LineaDeInvestigacion { get; set; }
        public string? Modalidad { get; set; }
        public string? Nombre { get; set; }
        public int? AcademiaId { get; set; }
        public int? PladeafeiId { get; set; }
        public int? ProyectoDeInvestigacionId { get; set; }
        public int? VinculacionId { get; set; }
        public string? Duracion { get; set; }
        public string? ExperienciaActual { get; set; }
        public string? JustificacionAlumnos { get; set; }

        public virtual Academium? Academia { get; set; }
        public virtual Pladeafei? Pladeafei { get; set; }
        public virtual ProyectoDeInvestigacion? ProyectoDeInvestigacion { get; set; }
        public virtual Vinculacion? Vinculacion { get; set; }
        public virtual ICollection<AlumnoTrabajoRecepcional> AlumnoTrabajoRecepcionals { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<ExamenDefensa> ExamenDefensas { get; set; }
        public virtual ICollection<TrabajoRecepcionalSinodalDelTrabajo> TrabajoRecepcionalSinodalDelTrabajos { get; set; }

        public virtual ICollection<Integrante> Integrantes { get; set; }
    }
}
