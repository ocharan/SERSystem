using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Archivo
    {
        public string? NombreArchivo { get; set; }
        public int IdArchivo { get; set; }
        public string? Direccion { get; set; }
        public int IdFuente { get; set; }
        public string? TipoContenido { get; set; }
        public string? Fuente { get; set; }
    }
}
