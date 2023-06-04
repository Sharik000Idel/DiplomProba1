using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Statususerroute
    {
        public Statususerroute()
        {
            Userroutes = new HashSet<Userroute>();
        }

        public int IdStatusUserRoute { get; set; }
        public string? StatusUserRoutecol { get; set; }

        public virtual ICollection<Userroute> Userroutes { get; set; }
    }
}
