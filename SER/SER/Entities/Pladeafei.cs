using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Pladeafei
    {
        public Pladeafei()
        {
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
            Lgacs = new HashSet<Lgac>();
        }

        public string? Accion { get; set; }
        public string? ObjetivoGeneral { get; set; }
        public string? Periodo { get; set; }
        public int PladeafeiId { get; set; }

        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }

        public virtual ICollection<Lgac> Lgacs { get; set; }
    }
}
