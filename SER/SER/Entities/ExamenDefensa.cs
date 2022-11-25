using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class ExamenDefensa
    {
        public ExamenDefensa()
        {
            Documentos = new HashSet<Documento>();
        }

        public DateTime? FechaAplicacion { get; set; }
        public int ExamenDefensaId { get; set; }
        public int TrabajoRecepcionalId { get; set; }
        public string? Estado { get; set; }

        public virtual TrabajoRecepcional TrabajoRecepcional { get; set; } = null!;

        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
