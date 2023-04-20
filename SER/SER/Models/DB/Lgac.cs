using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Lgac
    {
        public Lgac()
        {
            Pladeafeis = new HashSet<Pladeafei>();
            ProyectoDeInvestigacions = new HashSet<ProyectoDeInvestigacion>();
            Vinculacions = new HashSet<Vinculacion>();
        }

        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }
        public int LgacId { get; set; }
        public int? CuerpoAcademicoId { get; set; }

        public virtual ICollection<Pladeafei> Pladeafeis { get; set; }
        public virtual ICollection<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; }
        public virtual ICollection<Vinculacion> Vinculacions { get; set; }
    }
}
