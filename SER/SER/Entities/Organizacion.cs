using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Organizacion
    {
        public Organizacion()
        {
            SinodalDelTrabajos = new HashSet<SinodalDelTrabajo>();
            Vinculacions = new HashSet<Vinculacion>();
        }

        public string? Nombre { get; set; }
        public int OrganizacionId { get; set; }

        public virtual ICollection<SinodalDelTrabajo> SinodalDelTrabajos { get; set; }
        public virtual ICollection<Vinculacion> Vinculacions { get; set; }
    }
}
