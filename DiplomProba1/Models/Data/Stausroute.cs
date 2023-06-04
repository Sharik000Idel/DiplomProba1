using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Stausroute
    {
        public Stausroute()
        {
            Routes = new HashSet<Route>();
        }

        public int IdStausRoute { get; set; }
        public string? StausRoutecol { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
    }
}
