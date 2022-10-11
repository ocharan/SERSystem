using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class TrabajoRecepcional
    {
        public TrabajoRecepcional()
        {
            Documentos = new HashSet<Documento>();
            ExamenDefensas = new HashSet<ExamenDefensa>();
            TrabajoRecepcionalSinodalDelTrabajos = new HashSet<TrabajoRecepcionalSinodalDelTrabajo>();
            Alumnos = new HashSet<Alumno>();
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

        public virtual Academium? Academia { get; set; }
        public virtual Pladeafei? Pladeafei { get; set; }
        public virtual ProyectoDeInvestigacion? ProyectoDeInvestigacion { get; set; }
        public virtual Vinculacion? Vinculacion { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<ExamenDefensa> ExamenDefensas { get; set; }
        public virtual ICollection<TrabajoRecepcionalSinodalDelTrabajo> TrabajoRecepcionalSinodalDelTrabajos { get; set; }

        public virtual ICollection<Alumno> Alumnos { get; set; }
        public virtual ICollection<Integrante> Integrantes { get; set; }
    }
}
