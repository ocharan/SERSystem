using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class AlumnoExperienciaEducativa
    {
        public string AlumnoId { get; set; } = null!;
        public int ExperienciaEducativaId { get; set; }
        public string? Nombre { get; set; }
        public string? NombreExp { get; set; } 
        public virtual Alumno Alumno { get; set; } = null!;
        public virtual ExperienciaEducativa ExperienciaEducativa { get; set; } = null!;
    }
}
