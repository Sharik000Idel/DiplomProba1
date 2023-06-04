using DiplomProba1.Models;
using DiplomProba1.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DiplomProba1.Controllers
{
    public class CarController : Controller
    {
        
            public IActionResult UserCar()
            {
                var mySessionValue = HttpContext.Session.GetString("UserId");

                diplomdbContext diplomdbContext = new diplomdbContext();
                if (diplomdbContext.Users.First(x => x.IdUsers == Convert.ToInt32(mySessionValue)).CarId != null)
                {
                    Car a = diplomdbContext.Cars.
                        First(p => p.IdCar == diplomdbContext.Users.First(c => c.IdUsers == Convert.ToInt32(mySessionValue)).CarId);

                    ViewBag.Car = a;


                }
                return View();
            }

            public async Task<IActionResult> ChangeCarAdd(int id, string Gosnumber, string Namecar, int idtext, string textCar, TestImagecs testImagecs)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();

                int newIdCommentText = diplomdbContext.Commenttexts.ToList().OrderBy(p => p.IdCommentText).Last().IdCommentText + 1;



                Console.WriteLine(diplomdbContext.Cars.Count());
                Commenttext commenttexts = diplomdbContext.Commenttexts.First(t => t.IdCommentText == idtext);
                commenttexts.Text = textCar;
                diplomdbContext.Update(commenttexts);
                Car editCar = diplomdbContext.Cars.First(t => t.IdCar == id);

                if (testImagecs.UserImg != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await testImagecs.UserImg.CopyToAsync(stream);
                        editCar.CarImageNavigation.Image1 = stream.ToArray();
                    }
                }


                editCar.GosNumber = Gosnumber;
                editCar.NameCar = Namecar;
                diplomdbContext.Update(editCar);
                await diplomdbContext.SaveChangesAsync();

                return RedirectToAction("UserProfile", "User");
            }

            public async Task<IActionResult> NewCarAdd(string Gosnumber, string Namecar, string textCar, TestImagecs testImagecs)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();

                int newIdCommentText = diplomdbContext.Commenttexts.ToList().OrderBy(p => p.IdCommentText).Last().IdCommentText + 1;

                Console.WriteLine(diplomdbContext.Cars.Count());
                diplomdbContext.Commenttexts.Add(new Commenttext
                {
                    IdCommentText = newIdCommentText,
                    Text = textCar
                });

                int newUserImage = diplomdbContext.Images.ToList().OrderBy(p => p.IdImages).Last().IdImages + 1;
                DiplomProba1.Models.Data.Image newImage = null;



                if (testImagecs.UserImg != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await testImagecs.UserImg.CopyToAsync(stream);
                        newImage = new DiplomProba1.Models.Data.Image
                        {
                            IdImages = newUserImage,
                            Image1 = stream.ToArray()
                        };
                        Console.WriteLine(newImage.IdImages);
                    };
                }

                else
                {
                    var utf8 = new UTF8Encoding();
                    newImage = new DiplomProba1.Models.Data.Image
                    {
                        IdImages = newUserImage,
                        Image1 = diplomdbContext.Images.First().Image1
                    };
                    Console.WriteLine(newImage.IdImages);
                }



                Car NewCar = new Car
                {
                    IdCar = diplomdbContext.Cars.ToList().OrderBy(p => p.IdCar).Last().IdCar + 1,
                    GosNumber = Gosnumber,
                    NameCar = Namecar,
                    IdCommentCar = newIdCommentText,
                    CarImage = newImage.IdImages
                };
                diplomdbContext.Cars.Add(NewCar);
                diplomdbContext.Images.Add(newImage);
                User user = diplomdbContext.Users.First(u => u.IdUsers == Convert.
                ToInt32(HttpContext.Session.GetString("UserId")));
                user.CarId = NewCar.IdCar;
                user.IdRole = 2;
                diplomdbContext.SaveChanges();
                return RedirectToAction("UserProfile", "User");
            }
        
    }
}
