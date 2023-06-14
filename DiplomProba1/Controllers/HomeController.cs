using DiplomProba1.Models.Data;
using DiplomProba1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DinkToPdf.Contracts;

namespace DiplomProba1.Controllers
{
    public class HomeController : BaseController
    {

       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            diplomdbContext diplomdbContext = diplomdbContext.GetContext();
            ViewBag.VUsers = diplomdbContext.Users.Where(p=>p.IdRole == 2).OrderByDescending(p=>p.Estimation).Take(9).ToList();
            ViewBag.VRoutes = diplomdbContext.Routes.Where(r => r.IdStatusRoute == 1).OrderByDescending(p=>p.IdRout).Take(6).ToList();
            ViewBag.VCars = diplomdbContext.Cars.Take(6).ToList();
            ViewBag.DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.SiteComment = diplomdbContext.Comments.Where(p => p.IdUserComment == 8).ToList() ;
            return View();
        }

        public IActionResult About()
        {
            diplomdbContext diplomdbContext = diplomdbContext.GetContext();
            ViewBag.Comments = diplomdbContext.Comments.Where(p=>p.IdUserComment == 8).ToList();
            var mySessionValue = HttpContext.Session.GetString("UserId");
            if (mySessionValue != null && diplomdbContext.Comments.Where(p=>p.IdUserComment == 8 && p.IduserLeaveReview == Convert.ToInt32(mySessionValue)).Count() == 0)
            {
                ViewData["InRoute"] = true;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}