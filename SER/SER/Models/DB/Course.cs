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
        public string Nrc { get; set; } = null!;
        public string Period { get; set; } = null!;
        public string Section { get; set; } = null!;
        public bool IsOpen { get; set; }
        public int? ProfessorId { get; set; }
        public int? FileId { get; set; }

        public virtual CourseFile? File { get; set; }
        public virtual Professor? Professor { get; set; }
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
