using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class DireccionSinodalDelTrabajo
    {
        public int DireccionId { get; set; }
        public int SinodalDelTrabajoId { get; set; }
        public string? Tipo { get; set; }

        public virtual Direccion Direccion { get; set; } = null!;
        public virtual SinodalDelTrabajo SinodalDelTrabajo { get; set; } = null!;
    }
}
