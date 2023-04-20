using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class TrabajoRecepcionalSinodalDelTrabajo
    {
        public int SinodalDelTrabajoId { get; set; }
        public int TrabajoRecepcionalId { get; set; }
        public string? TipoSinodal { get; set; }

        public virtual SinodalDelTrabajo SinodalDelTrabajo { get; set; } = null!;
        public virtual TrabajoRecepcional TrabajoRecepcional { get; set; } = null!;
    }
}
