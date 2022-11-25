using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class SinodalDelTrabajo
    {
        public SinodalDelTrabajo()
        {
            DireccionSinodalDelTrabajos = new HashSet<DireccionSinodalDelTrabajo>();
            TrabajoRecepcionalSinodalDelTrabajos = new HashSet<TrabajoRecepcionalSinodalDelTrabajo>();
        }

        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public int SinodalDelTrabajoId { get; set; }
        public string? Nombre { get; set; }
        public int? NumeroDePersonal { get; set; }
        public int OrganizacionId { get; set; }

        public virtual Organizacion Organizacion { get; set; } = null!;
        public virtual ICollection<DireccionSinodalDelTrabajo> DireccionSinodalDelTrabajos { get; set; }
        public virtual ICollection<TrabajoRecepcionalSinodalDelTrabajo> TrabajoRecepcionalSinodalDelTrabajos { get; set; }
    }
}
