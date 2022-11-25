using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Integrante
    {
        public Integrante()
        {
            TrabajoRecepcionals = new HashSet<TrabajoRecepcional>();
        }

        public string? NumeroDePersonal { get; set; }
        public string? Tipo { get; set; }
        public int IntegranteId { get; set; }
        public string? Nombre { get; set; }
        public int CuerpoAcademicoId { get; set; }

        public virtual CuerpoAcademico CuerpoAcademico { get; set; } = null!;

        public virtual ICollection<TrabajoRecepcional> TrabajoRecepcionals { get; set; }
    }
}
