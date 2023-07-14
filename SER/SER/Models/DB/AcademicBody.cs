using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class AcademicBody
    {
        public AcademicBody()
        {
            AcademicBodyLgacs = new HashSet<AcademicBodyLgac>();
            AcademicBodyMembers = new HashSet<AcademicBodyMember>();
        }

        public int AcademicBodyId { get; set; }
        public string AcademicBodyKey { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Ies { get; set; } = null!;
        public string ConsolidationDegree { get; set; } = null!;
        public string Discipline { get; set; } = null!;

        public virtual ICollection<AcademicBodyLgac> AcademicBodyLgacs { get; set; }
        public virtual ICollection<AcademicBodyMember> AcademicBodyMembers { get; set; }
    }
}
