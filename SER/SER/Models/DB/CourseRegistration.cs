using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class CourseRegistration
    {
        public int CourseRegistrationId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int? Score { get; set; }
        public string RegistrationType { get; set; } = null!;

        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
