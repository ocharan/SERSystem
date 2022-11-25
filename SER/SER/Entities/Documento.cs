using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Documento
    {
        public Documento()
        {
            ExamenDefensas = new HashSet<ExamenDefensa>();
        }

        public int DocumentoId { get; set; }
        public int? TrabajoRecepcionalId { get; set; }
        public string? Notas { get; set; }
        public int? TipoDocumentoId { get; set; }

        public virtual TipoDocumento? TipoDocumento { get; set; }
        public virtual TrabajoRecepcional? TrabajoRecepcional { get; set; }

        public virtual ICollection<ExamenDefensa> ExamenDefensas { get; set; }
    }
}
