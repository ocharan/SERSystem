using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class AlumnoTrabajoRecepcionalProyectoGuiadoView
    {
        public string? CorreoElectronico { get; set; }
        public string Matricula { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Estado { get; set; }
        public DateTime? Fechadeinicio { get; set; }
        public string? Modalidad { get; set; }
        public string? ExperienciaEducativa { get; set; }
        public int ExperienciaEducativaId { get; set; }
        public int TrabajoRecepcionalId { get; set; }
    }
}
