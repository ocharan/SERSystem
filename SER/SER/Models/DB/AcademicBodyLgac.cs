using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class AcademicBodyLgac
    {
        public int AcademicBodyLgac1 { get; set; }
        public int AcademicBodyId { get; set; }
        public int LgacId { get; set; }

        public virtual AcademicBody AcademicBody { get; set; } = null!;
        public virtual Lgac Lgac { get; set; } = null!;
    }
}
