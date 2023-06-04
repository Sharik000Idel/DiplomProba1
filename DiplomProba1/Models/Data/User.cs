using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class User
    {
        public User()
        {
            CommentIdUserCommentNavigations = new HashSet<Comment>();
            CommentIduserLeaveReviewNavigations = new HashSet<Comment>();
            Routes = new HashSet<Route>();
            Userroutes = new HashSet<Userroute>();
        }

        public int IdUsers { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public string? Lastname { get; set; }
        public string Email { get; set; } = null!;
        public string? Login { get; set; }
        public string Password { get; set; } = null!;
        public int IdRole { get; set; }
        public int? IdCommentText { get; set; }
        public decimal? Estimation { get; set; }
        public int? CarId { get; set; }
        public int? UserImg { get; set; }

        public DateOnly? DateRegistration { get; set; }

        public virtual Car? Car { get; set; }
        public virtual Commenttext? IdCommentTextNavigation { get; set; }
        public virtual Role IdRoleNavigation { get; set; } = null!;
        public virtual Image? UserImgNavigation { get; set; }
        public virtual ICollection<Comment> CommentIdUserCommentNavigations { get; set; }
        public virtual ICollection<Comment> CommentIduserLeaveReviewNavigations { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
        public virtual ICollection<Userroute> Userroutes { get; set; }


        public string Fulname
        {
            get { return Surname + " " + Name + " " + Lastname; }
        }
        public string FormatImage(string a)
        {
            switch (a.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                default:
                    return "png";
            }
        }

        public string PhotoUser { get { string a = System.Convert.ToBase64String(UserImgNavigation.Image1); return "data:image/" + FormatImage(a.Substring(0, 5)) + ";base64," + a; } }
        public string Age { get { return (DateTime.Today.Year - Birthday.Year).ToString(); } }
    }
}
