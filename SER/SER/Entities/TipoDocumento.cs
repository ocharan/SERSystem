using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Documentos = new HashSet<Documento>();
        }

        public string? NombreDocumento { get; set; }
        public string? ExperienciaEducativa { get; set; }
        public int IdTipo { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
