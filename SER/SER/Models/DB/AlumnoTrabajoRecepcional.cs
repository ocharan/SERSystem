using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class AlumnoTrabajoRecepcional
    {
        public string AlumnoId { get; set; } = null!;
        public int TrabajoRecepcionalId { get; set; }
        public string? Nombre { get; set; }

        public virtual TrabajoRecepcional TrabajoRecepcional { get; set; } = null!;
    }
}
