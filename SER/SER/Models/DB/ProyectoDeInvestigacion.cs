using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class ProyectoDeInvestigacion
    {
        public ProyectoDeInvestigacion()
        {
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
            Lgacs = new HashSet<Lgac>();
        }

        public DateTime? FechaInicio { get; set; }
        public string? Nombre { get; set; }
        public int ProyectoDeInvestigacionId { get; set; }
        public int CuerpoAcademicoId { get; set; }

        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }

        public virtual ICollection<Lgac> Lgacs { get; set; }
    }
}
