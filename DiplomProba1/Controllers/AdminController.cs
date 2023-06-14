using DiplomProba1.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace DiplomProba1.Controllers
{
    public class AdminController : BaseController
    {
        public IActionResult СonfirmationDriverPage()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            ViewBag.Drivers = diplomdbContext.Users.Where(u => u.IdRole == 4).ToList();
            return View();
        }

        public IActionResult ResponseDriver(int id , int status)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            User us = diplomdbContext.Users.First(p => p.IdUsers == id);
            us.IdRole = status;
            if (status == 1)
            {
                Car c = diplomdbContext.Cars.First(p => p.IdCar == us.CarId);
                 us.CarId = null;
                diplomdbContext.Remove(c);
            }
            diplomdbContext.SaveChanges();
            return RedirectToAction("СonfirmationDriverPage");
        }


        public IActionResult BadDriverPage()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            ViewBag.Drivers = diplomdbContext.Users.Where(u => u.Estimation < 3).ToList();
            return View();
        }

        public IActionResult DeleteDriver(int id, int status)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            User us = diplomdbContext.Users.First(p => p.IdUsers == id);
            us.IdRole = status;
            diplomdbContext.SaveChanges();
            return RedirectToAction("СonfirmationDriverPage");
        }

        public IActionResult BlockDriverPage()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            ViewBag.Drivers = diplomdbContext.Users.Where(u => u.IdUsers == 5 && u.IdUsers == 7).ToList();
            return View();
        }
    }
}
