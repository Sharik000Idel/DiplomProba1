using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int IdRole { get; set; }
        public string Role1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
