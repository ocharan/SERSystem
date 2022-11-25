using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class Expediente
    {
        public string? NombreAlumno { get; set; }
        public string Matricula { get; set; } = null!;
        public string? CorreoElectronico { get; set; }
        public string? Nombre { get; set; }
        public int TrabajoRecepcionalId { get; set; }
        public string? Estado { get; set; }
        public DateTime? Fechadeinicio { get; set; }
        public string? Modalidad { get; set; }
        public string? LineaDeInvestigacion { get; set; }
    }
}
