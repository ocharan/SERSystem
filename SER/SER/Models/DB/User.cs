using System;
using System.Collections.Generic;

namespace SER.Models.DB
{
    public partial class User
    {
        public User()
        {
            Professors = new HashSet<Professor>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? RecoveryToken { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }
    }
}
