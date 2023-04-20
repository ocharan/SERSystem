using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Direccion
    {
        public Direccion()
        {
            DireccionSinodalDelTrabajos = new HashSet<DireccionSinodalDelTrabajo>();
        }

        public DateTime? FechaInicio { get; set; }
        public int DireccionId { get; set; }
        public int ExperienciaEducativaId { get; set; }

        public virtual ICollection<DireccionSinodalDelTrabajo> DireccionSinodalDelTrabajos { get; set; }
    }
}
