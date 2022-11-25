using System;
using System.Collections.Generic;

namespace SER.Entities
{
    public partial class PlanDeCurso
    {
        public string? ObjetivoGeneral { get; set; }
        public int PlanDeCursoId { get; set; }
        public int ExperienciaEducativaId { get; set; }

        public virtual ExperienciaEducativa ExperienciaEducativa { get; set; } = null!;
    }
}
