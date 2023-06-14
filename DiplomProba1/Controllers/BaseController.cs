using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DiplomProba1.Models.Data;
namespace DiplomProba1.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                diplomdbContext diplomdbContext = diplomdbContext.GetContext();
                var mySessionValue = HttpContext.Session.GetString("UserId");

                int a = diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 1 && r.IdUser == Convert.ToInt32(mySessionValue)).ToList().Count();
                //все маршруты пользоавтеля
                Console.WriteLine("сооб от базы 0 кол сообщений " + a);
                a += diplomdbContext.Userroutes.Where(r => r.IdUser == Convert.ToInt32(mySessionValue) && r.ReadMess == null && r.StatusUserRouteId != 3 && (r.IdRoutNavigation.IdStatusRoute == 1 || r.IdRoutNavigation.IdStatusRoute == 4)).ToList().Count();

                Console.WriteLine("сооб от базы 1 кол сообщений " + a);
                List<DiplomProba1.Models.Data.Route> routes = diplomdbContext.Userroutes.Where(r => r.IdUser == Convert.ToInt32(mySessionValue) && r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3 && r.IdRoutNavigation.DataTimeStart.AddDays(1) > DateTime.Now).Select(p => p.IdRoutNavigation).ToList();

                List<User> users2 = new List<User>();
                foreach (DiplomProba1.Models.Data.Route ro in routes)
                {
                    foreach (User us in ro.AllUsersGetRoute())
                    {
                        
                        if (diplomdbContext.Comments.FirstOrDefault(p => p.IdUserComment == us.IdUsers && p.IduserLeaveReview == Convert.ToInt32(mySessionValue) && p.IdUserComment != Convert.ToInt32(mySessionValue)) == null)
                        {
                            
                            if (us.IdUsers != Convert.ToInt32(mySessionValue))
                            {
                                users2.Add(us);
                            }
                        }
                    }
                }

                users2 = users2.GroupBy(x => x.IdUsers).Select(y => y.First()).ToList();
                a += users2.Count();
                Console.WriteLine("сооб от базы  2 кол сообщений " + a);

                //Console.WriteLine("user " + usereroute.IdUser);
                //ViewBag.RequestUser = null;
                //if (usereroute != null)
                //{
                //    if (diplomdbContext.Comments.FirstOrDefault(p => p.IduserLeaveReview == Convert.ToInt32(HttpContext.Session.GetString("UserId")) && p.IdUserComment == usereroute.IdUser) == null)
                //    {
                //        ViewBag.RequestUser = usereroute;
                //    }
                //}
                List<DiplomProba1.Models.Data.Route> route = diplomdbContext.Routes.Where(p => (p.IdStatusRoute == 1 || p.IdStatusRoute == 4) && p.IdUser == Convert.ToInt32(mySessionValue) && p.DataTimeStart > DateTime.Now).ToList();
                foreach (DiplomProba1.Models.Data.Route item in route)
                {
                    a += item.UsersRouteGetRoute().Count();
                }
                Console.WriteLine("сооб от базы 3 кол сообщений  " + a);
                if (diplomdbContext.Users.FirstOrDefault(p => p.IdUsers == Convert.ToInt32(mySessionValue)).IdRole == 6)
                {
                    
                    a += 1;
                }
                if (diplomdbContext.Users.FirstOrDefault(p => p.IdUsers == Convert.ToInt32(mySessionValue)).IdRole == 5)
                {
                    
                    a += 1;
                }
                if (diplomdbContext.Users.FirstOrDefault(p => p.IdUsers == Convert.ToInt32(mySessionValue)).IdRole == 2)
                {

                    DiplomProba1.Models.Data.Route RouteVod = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute != 3 && p.IdStatusRoute != 2).OrderBy(p => p.DataTimeStart).FirstOrDefault();
                    if (RouteVod != null && RouteVod.DataTimeStart <= DateTime.Now)
                    {
                        a++;
                    }

                }


                HttpContext.Session.SetString("CountMess", a.ToString());
            }


           
        }
    }
}
