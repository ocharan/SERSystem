using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Usuario
    {
        public string? NombreUsuario { get; set; }
        public string? Contra { get; set; }
        public string? Tipo { get; set; }
        public int IdUsuario { get; set; }
    }
}
