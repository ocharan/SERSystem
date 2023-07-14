using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Professor
    {
        public Professor()
        {
            AcademicBodyMembers = new HashSet<AcademicBodyMember>();
            Courses = new HashSet<Course>();
        }

        public int ProfessorId { get; set; }
        public string FullName { get; set; } = null!;
        public string AcademicDegree { get; set; } = null!;
        public string StudyField { get; set; } = null!;
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<AcademicBodyMember> AcademicBodyMembers { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
