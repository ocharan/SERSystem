using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class ExperienciaEducativa
    {
        public ExperienciaEducativa()
        {
            Direccions = new HashSet<Direccion>();
            PlanDeCursos = new HashSet<PlanDeCurso>();
            Alumnos = new HashSet<Alumno>();
        }

        public int? EstadoAbierto { get; set; }
        public string? Nombre { get; set; }
        public string? Nrc { get; set; }
        public string? Periodo { get; set; }
        public string? Seccion { get; set; }
        public int ExperienciaEducativaId { get; set; }
        public int? ProfesorId { get; set; }

        public virtual Profesor? Profesor { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<PlanDeCurso> PlanDeCursos { get; set; }

        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
