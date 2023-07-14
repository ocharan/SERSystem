using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class TrabajoRecepcionalIntegrante
    {
        public int IntegranteId { get; set; }
        public int TrabajoRecepcionalId { get; set; }

        public virtual TrabajoRecepcional TrabajoRecepcional { get; set; } = null!;
    }
}
