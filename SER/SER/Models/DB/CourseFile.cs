using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class CourseFile
    {
        public CourseFile()
        {
            Courses = new HashSet<Course>();
        }

        public int FileId { get; set; }
        public string Path { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
    }
}
