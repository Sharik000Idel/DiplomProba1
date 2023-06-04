using DiplomProba1.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace DiplomProba1.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult ViewRequest()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            var mySessionValue = HttpContext.Session.GetString("UserId");

            //List<DiplomWebsitePoputchici.Models.Data.Userroute> userroutes = diplomdbContext.Userroutes.Where(m => m.IdRoutNavigation.IdUser == Convert.ToInt32(mySessionValue)).ToList();

            List<DiplomProba1.Models.Data.Route> userroute = diplomdbContext.Routes.Where(p => p.IdStatusRoute == 1 && p.IdUser == Convert.ToInt32(mySessionValue) && p.DataTimeStart > DateTime.Now ).ToList();

            //List<DiplomProba1.Models.Data.Route> olduserroute = diplomdbContext.Routes.Where(p => p.IdStatusRoute == 1 && p.IdUser == Convert.ToInt32(mySessionValue) && p.DataTimeStart > DateTime.Now).ToList();
            Console.WriteLine("количество заявок " + userroute.Count());
            ViewBag.UserRoute = userroute;
            return View();
        }

        

        public RedirectToActionResult RequestForRoute(int id ,int? count)
        {
            if (count == null && count == 0 )
            {
                count = 1;
            }
            if (HttpContext.Session.GetString("UserId") != null)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();
                diplomdbContext.Userroutes.Add(new Userroute
                {
                    IdUserroutes = diplomdbContext.Userroutes.OrderBy(o => o.IdUserroutes).Last().IdUserroutes + 1,
                    IdRout = id,
                    IdUser = Convert.ToInt32(HttpContext.Session.GetString("UserId")),
                    StatusUserRouteId = 3,
                    Bookcount = count
                });
                diplomdbContext.SaveChangesAsync();
                return RedirectToAction("FoundRoutes", "Route");
            }
            else
            {
                return RedirectToAction("Aauthorization", "User");
            }
        }

        public async Task<IActionResult> Accept(int idUser, int idroute , int idrequest)//ответ ня заявку
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            
            if (diplomdbContext.Userroutes.First(p => p.IdUser == idUser && p.IdRout == idroute) != null)
            {
                diplomdbContext.Userroutes.First(p => p.IdUser == idUser && p.IdRout == idroute).StatusUserRouteId = idrequest;
                await diplomdbContext.SaveChangesAsync();
            }
            return RedirectToAction("ViewRequest");

        }

        public async Task<IActionResult> RefuseRoute(int id)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            diplomdbContext.Userroutes.Remove(diplomdbContext.Userroutes.First(p => p.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId")) && p.IdRout == id));
            await diplomdbContext.SaveChangesAsync();
            return RedirectToAction("UserProfile" , "User");
        }
    }
}
