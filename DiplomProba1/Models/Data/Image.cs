using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Image
    {
        public Image()
        {
            Cars = new HashSet<Car>();
            Users = new HashSet<User>();
        }

        public int IdImages { get; set; }
        public byte[]? Image1 { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public string FormatImage(string a)
        {
            switch (a.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                default:
                    return string.Empty;
            }
        }

        public string Photo { get { string a = System.Convert.ToBase64String(Image1); return "data:image/" + FormatImage(a.Substring(0, 5)) + ";base64," + a; } }

    }
}
