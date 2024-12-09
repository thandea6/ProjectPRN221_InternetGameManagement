using System;
using System.Collections.Generic;

namespace ProjectPRN221_InternetGameManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? Time { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
