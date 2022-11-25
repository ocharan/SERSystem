using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class AlumnoTrabajoRecepcional
    {
        public string AlumnoId { get; set; } = null!;
        public int TrabajoRecepcionalId { get; set; }
        public string? Nombre { get; set; }
        public virtual Alumno Alumno { get; set; } = null!;
        public virtual TrabajoRecepcional TrabajoRecepcional { get; set; } = null!;
    }
}
