using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Lgac
    {
        public Lgac()
        {
            AcademicBodyLgacs = new HashSet<AcademicBodyLgac>();
            Pladeafeis = new HashSet<Pladeafei>();
            ProyectoDeInvestigacions = new HashSet<ProyectoDeInvestigacion>();
            Vinculacions = new HashSet<Vinculacion>();
        }

        public int LgacId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<AcademicBodyLgac> AcademicBodyLgacs { get; set; }

        public virtual ICollection<Pladeafei> Pladeafeis { get; set; }
        public virtual ICollection<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }
        public virtual ICollection<Vinculacion> Vinculacions { get; set; }
    }
}
