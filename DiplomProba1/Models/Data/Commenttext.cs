using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Commenttext
    {
        public Commenttext()
        {
            Cars = new HashSet<Car>();
            Comments = new HashSet<Comment>();
            Routes = new HashSet<Route>();
            Users = new HashSet<User>();
        }

        public int IdCommentText { get; set; }
        public string? Text { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
