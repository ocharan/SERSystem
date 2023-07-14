using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class AcademicBodyMember
    {
        public int AcademicBodyMemberId { get; set; }
        public int AcademicBodyId { get; set; }
        public int ProfessorId { get; set; }
        public string Role { get; set; } = null!;

        public virtual AcademicBody AcademicBody { get; set; } = null!;
        public virtual Professor Professor { get; set; } = null!;
    }
}
