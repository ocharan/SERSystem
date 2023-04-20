using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Course
    {
        public Course()
        {
            CourseRegistrations = new HashSet<CourseRegistration>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; } = null!;
        public int Nrc { get; set; }
        public string Period { get; set; } = null!;
        public int Section { get; set; }
        public bool IsOpen { get; set; }
        public int? ProfessorId { get; set; }

        public virtual Professor? Professor { get; set; }
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
