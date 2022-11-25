using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class ExperienciaEducativa
    {
        public ExperienciaEducativa()
        {
            AlumnoExperienciaEducativas = new HashSet<AlumnoExperienciaEducativa>();
            Direccions = new HashSet<Direccion>();
            PlanDeCursos = new HashSet<PlanDeCurso>();
        }

        public int? EstadoAbierto { get; set; }
        public string? Nombre { get; set; }
        public string? Nrc { get; set; }
        public string? Periodo { get; set; }
        public string? Seccion { get; set; }
        public int ExperienciaEducativaId { get; set; }
        public int? ProfesorId { get; set; }

        public virtual Profesor? Profesor { get; set; }
        public virtual ICollection<AlumnoExperienciaEducativa> AlumnoExperienciaEducativas { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<PlanDeCurso> PlanDeCursos { get; set; }
    }
}
