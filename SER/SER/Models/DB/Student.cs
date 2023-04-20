using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class Student
    {
        public Student()
        {
            CourseRegistrations = new HashSet<CourseRegistration>();
        }

        public int StudentId { get; set; }
        public string Enrollment { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
