using System;
using System.Collections.Generic;

namespace DiplomProba1.Models.Data
{
    public partial class Route
    {
        public Route()
        {
            Userroutes = new HashSet<Userroute>();
        }

        public int IdRout { get; set; }
        public int IdUser { get; set; }
        public string? BeginRoute { get; set; }
        public string EndRoute { get; set; } = null!;
        public DateTime DataTimeStart { get; set; }
        public int? Cost { get; set; }
        public int? IdCommentText { get; set; }
        public int? CountPassagir { get; set; }
        public int? IdStatusRoute { get; set; }

        public DateOnly? DateAddedRoude { get; set; }

        public virtual Commenttext? IdCommentTextNavigation { get; set; }
        public virtual Stausroute? IdStatusRouteNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<Userroute> Userroutes { get; set; }


        public string DataRoute {
            get { return DataTimeStart.ToString("dd MMMM") ; }
        }
        public string TimeRoute
        {
            get { return DataTimeStart.ToString("t"); }
        }

        public List<User> UsersGetRoute()
        {

            diplomdbContext diplomdbContext = new diplomdbContext();
            return diplomdbContext.Userroutes.Where(m => m.IdRout == IdRout && m.StatusUserRouteId == 3).Select(m => m.IdUserNavigation).ToList();


        }

        public List<Userroute> UsersRouteGetRoute()
        {

            diplomdbContext diplomdbContext = new diplomdbContext();
            return diplomdbContext.Userroutes.Where(m => m.IdRout == IdRout && m.StatusUserRouteId == 3).ToList();


        }


        public List<User> AllUsersGetRoute()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            return diplomdbContext.Userroutes.Where(m => m.IdRout == IdRout && m.StatusUserRouteId == 1).Select(m => m.IdUserNavigation).ToList();
        }

        public List<Userroute> AllUsersGetUserRoute()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            return diplomdbContext.Userroutes.Where(m => m.IdRout == IdRout && m.StatusUserRouteId == 1).ToList();
        }

        public  int? CountFreeRoute()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            return (CountPassagir - diplomdbContext.Userroutes.Where(m => m.IdRout == IdRout && m.StatusUserRouteId == 1).Select(p=>p.Bookcount).ToList().Sum());
        }

        public static IEnumerable<IGrouping<string , string>> MorePopularCity()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            List<string> city = diplomdbContext.Routes.Select(c=>c.BeginRoute).ToList();
            city.AddRange(diplomdbContext.Routes.Select(c => c.EndRoute).ToList());
            var most = city.GroupBy(x => x).OrderByDescending(x => x.Count()).ToList().Take(4);

            


            city = most.Select(x => x.Key).ToList();
            return most;
        }
    }
}
