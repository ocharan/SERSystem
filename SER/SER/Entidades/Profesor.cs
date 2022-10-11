using System;
using System.Collections.Generic;

namespace SER.Entidades
{
    public partial class Profesor
    {
        public Profesor()
        {
            ExperienciaEducativas = new HashSet<ExperienciaEducativa>();
        }

        public string? Nombre { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NumeroDePersonal { get; set; }
        public int ProfesorId { get; set; }

        public virtual ICollection<ExperienciaEducativa> ExperienciaEducativas { get; set; }
    }
}
