using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class Alumno
    {
        public Alumno()
        {
            ExperienciaEducativas = new HashSet<ExperienciaEducativa>();
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
        }

        public string? CorreoElectronico { get; set; }
        public string Matricula { get; set; } = null!;
        public string? Nombre { get; set; }

        public virtual ICollection<ExperienciaEducativa> ExperienciaEducativas { get; set; }
        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }
    }
}
