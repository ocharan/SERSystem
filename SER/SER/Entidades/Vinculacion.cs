using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class Vinculacion
    {
        public Vinculacion()
        {
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
            Lgacs = new HashSet<Lgac>();
        }

        public DateTime? FechaDeInicioDeConvenio { get; set; }
        public int VinculacionId { get; set; }
        public int OrganizacionIid { get; set; }

        public virtual Organizacion OrganizacionI { get; set; } = null!;
        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }

        public virtual ICollection<Lgac> Lgacs { get; set; }
    }
}
