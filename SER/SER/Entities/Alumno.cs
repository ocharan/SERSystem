using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Alumno
    {
        public Alumno()
        {
            AlumnoExperienciaEducativas = new HashSet<AlumnoExperienciaEducativa>();
            AlumnoTrabajoRecepcionals = new HashSet<AlumnoTrabajoRecepcional>();
        }

        public string? CorreoElectronico { get; set; }
        public string Matricula { get; set; } = null!;
        public string? Nombre { get; set; }

        public virtual ICollection<AlumnoExperienciaEducativa> AlumnoExperienciaEducativas { get; set; }
        public virtual ICollection<AlumnoTrabajoRecepcional> AlumnoTrabajoRecepcionals { get; set; }
    }
}
