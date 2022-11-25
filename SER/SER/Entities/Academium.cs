using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Academium
    {
        public Academium()
        {
            PlanDeTrabajos = new HashSet<PlanDeTrabajo>();
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
        }

        public int AcademiaId { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<PlanDeTrabajo> PlanDeTrabajos { get; set; }
        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }
    }
}
