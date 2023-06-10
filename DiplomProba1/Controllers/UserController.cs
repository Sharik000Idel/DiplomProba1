using Microsoft.AspNetCore.Mvc;
using DiplomProba1.Models.Data;
using System.Globalization;
using System.Text;
using DiplomProba1.Models;
using Microsoft.Extensions.FileProviders;

namespace DiplomProba1.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Registration()//регистрация 
        {
            if (TempData["EmailTrue"] != null)
            {
                ViewData["EmailTrue"] = "Пользователь с таким Email существеут";
            }
            var mySessionValue = HttpContext.Session.GetString("UserId");
            if (mySessionValue != null)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();
                User user = diplomdbContext.Users.First(x => x.IdUsers == Convert.ToInt32(mySessionValue));
                Role rol = diplomdbContext.Roles.First(r => r.IdRole == user.IdRole);
                Commenttext commenttext = diplomdbContext.Commenttexts.First(x => x.IdCommentText == user.IdCommentText);
                ViewBag.UserInfo = user;
                ViewBag.UserStatusInfo = rol;
                ViewBag.UserCommentAbout = commenttext;

                
            }
            return View();
        }

        public IActionResult Aauthorization()//авторизация 
        {
            
            var mySessionValue = HttpContext.Session.GetString("UserId");
            if (mySessionValue != null)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();
                User user = diplomdbContext.Users.First(x => x.IdUsers == Convert.ToInt32(mySessionValue));
                Role rol = diplomdbContext.Roles.First(r => r.IdRole == user.IdRole);
                Commenttext commenttext = diplomdbContext.Commenttexts.First(x => x.IdCommentText == user.IdCommentText);
                ViewBag.UserInfo = user;
                ViewBag.UserStatusInfo = rol;
                ViewBag.UserCommentAbout = commenttext;
            }
            return View();
        }

        public IActionResult UserProfile()//странцица пользователя
        {
            return RedirectToAction("UserProfileVer2");
            Console.WriteLine(DateTime.Now);
            var mySessionValue = HttpContext.Session.GetString("UserId");
            if (mySessionValue != null)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();
                Console.WriteLine(diplomdbContext.Roles.Count());
                User user = diplomdbContext.Users.First(x => x.IdUsers == Convert.ToInt32(mySessionValue));
                ViewBag.UserInfo = user;
                ViewBag.Comments = diplomdbContext.Comments.Where(u => u.IdUserComment == user.IdUsers).ToList();
                ViewBag.MyComments = diplomdbContext.Comments.Where(u => u.IduserLeaveReview == user.IdUsers).ToList();

                List<DiplomProba1.Models.Data.Userroute> routes = diplomdbContext.Userroutes.Where(m => m.IdUser == Convert.ToInt32(mySessionValue) && m.StatusUserRouteId == 1).ToList();
                List<DiplomProba1.Models.Data.Userroute> routes1 = diplomdbContext.Userroutes.Where(m => m.IdUser == Convert.ToInt32(mySessionValue) && m.StatusUserRouteId == 3).ToList();

                List<DiplomProba1.Models.Data.Route> routesVodit = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute == 1).ToList();
                ViewData["CountReqwest"] = diplomdbContext.Userroutes.Where(m =>m.StatusUserRouteId == 3  && routesVodit.Contains(m.IdRoutNavigation)).Count();

                DiplomProba1.Models.Data.Route RouteVod = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute == 2).FirstOrDefault();
                if (RouteVod == null)
                {
                    RouteVod = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute != 3).OrderBy(p => p.DataTimeStart).FirstOrDefault();
                }
                ViewBag.RouteVod = RouteVod;
                ViewBag.FutereRoutes = routes.Where(d => d.IdRoutNavigation.DataTimeStart > DateTime.Now).ToList();
                ViewBag.RequestRoutes = routes1.Where(d => d.IdRoutNavigation.DataTimeStart > DateTime.Now).ToList();
                ViewBag.ArchivRoutes = routes.Where(d => d.IdRoutNavigation.DataTimeStart < DateTime.Now).ToList();
                return View();
            }
            return RedirectToAction("Aauthorization");
        }

        public IActionResult UserProfileVer2()//странцица пользователя
        {
            Console.WriteLine(DateTime.Now);
            var mySessionValue = HttpContext.Session.GetString("UserId");
            if (mySessionValue != null)
            {
                
                diplomdbContext diplomdbContext = new diplomdbContext();
                Console.WriteLine(diplomdbContext.Roles.Count());
                User user = diplomdbContext.Users.First(x => x.IdUsers == Convert.ToInt32(mySessionValue));
                ViewBag.UserInfo = user;
                ViewBag.Comments = diplomdbContext.Comments.Where(u => u.IdUserComment == user.IdUsers).ToList();
                ViewBag.MyComments = diplomdbContext.Comments.Where(u => u.IduserLeaveReview == user.IdUsers).ToList();

                List<DiplomProba1.Models.Data.Userroute> routes = diplomdbContext.Userroutes.Where(m => m.IdUser == Convert.ToInt32(mySessionValue) && m.StatusUserRouteId == 1).ToList();
                List<DiplomProba1.Models.Data.Userroute> routes1 = diplomdbContext.Userroutes.Where(m => m.IdUser == Convert.ToInt32(mySessionValue) && m.StatusUserRouteId == 3).ToList();

                List<DiplomProba1.Models.Data.Route> routesVodit = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute == 1).ToList();
                ViewData["CountReqwest"] = diplomdbContext.Userroutes.Where(m => m.StatusUserRouteId == 3 && routesVodit.Contains(m.IdRoutNavigation)).Count();
                if (diplomdbContext.Routes.Where(m => m.IdUser == Convert.ToInt32(mySessionValue)).Count() >= 3)
                {
                    ViewData["MaxRoute"] = "true";
                }
                


                DiplomProba1.Models.Data.Route RouteVod = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute == 2).FirstOrDefault();
                if (RouteVod == null)
                {
                    RouteVod = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute != 3 && p.IdStatusRoute != 2).OrderBy(p => p.DataTimeStart).FirstOrDefault();
                }
                ViewBag.RouteVod = RouteVod;
                ViewBag.FutereRoutes = routes.Where(d => d.IdRoutNavigation.DataTimeStart > DateTime.Now).ToList();
                ViewBag.RequestRoutes = routes1.Where(d => d.IdRoutNavigation.DataTimeStart > DateTime.Now).ToList();
                ViewBag.ArchivRoutes = routes.Where(d => d.IdRoutNavigation.DataTimeStart < DateTime.Now).ToList();
                return View();
            }
            return RedirectToAction("Aauthorization");
        }


        public IActionResult LogIn(string email, string password) //вход
        {
            diplomdbContext diplomdbContext = diplomdbContext.GetContext();
            


            if (diplomdbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password) != null)
            {
                User us = diplomdbContext.Users.First(u => u.Email == email && u.Password == password);

                

                //int CountMess = diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 1 && r.IdUser == us.IdUsers).Count();
                //Console.WriteLine("предстоящие поездки" + CountMess);
                //CountMess += diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3 && r.IdUser == us.IdUsers && r.IdRoutNavigation.DataTimeStart.AddDays(1) < DateTime.Now).Count();
                //Console.WriteLine("нидавние поездки" + CountMess);
                //if (us.IdRole == 2)
                //{
                //    List<DiplomProba1.Models.Data.Route> routesVodit = diplomdbContext.Routes.Where(p => p.IdUser == us.IdUsers && p.IdStatusRoute == 1).ToList();
                //    CountMess += diplomdbContext.Userroutes.Where(m => m.StatusUserRouteId == 3 && routesVodit.Contains(m.IdRoutNavigation)).Count();
                //    Console.WriteLine("колчиесвсто заявок" + CountMess);
                //}
                //Console.WriteLine("всего" + CountMess);
                //HttpContext.Session.SetString("CountMess", CountMess.ToString());


                HttpContext.Session.SetString("UserId", us.IdUsers.ToString());
                HttpContext.Session.SetString("UserImage", System.Convert.ToBase64String(us.UserImgNavigation.Image1));
                HttpContext.Session.SetString("UserImageId", us.UserImgNavigation.IdImages.ToString());
                var mySessionValue = HttpContext.Session.GetString("UserId");
                Console.WriteLine(mySessionValue);
                return RedirectToAction("UserProfile");
            }
            return RedirectToAction("Aauthorization");
        }
        public async Task<IActionResult> RegistrationForm( //создание нового пользователя
        string name,
        string surname,
        string lastName,
        string Birthday,
        string Email,
        string password,
        string numer,
        string commentAbout,
        int IdRole = 1,
        decimal ecimation = 0.0m)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            if (diplomdbContext.Users.Where(e => e.Email.Trim() == Email.Trim()).ToList().Count != 0)
            {
                TempData["EmailTrue"] = "";
                return RedirectToAction("Registration");
            }
            
                int newIdCommentText = diplomdbContext.Commenttexts.ToList().OrderBy(p => p.IdCommentText).Last().IdCommentText + 1;

                Commenttext commenttext = new Commenttext
                {
                    IdCommentText = newIdCommentText,
                    Text = commentAbout
                };


                int newUserImage = diplomdbContext.Images.ToList().OrderBy(p => p.IdImages).Last().IdImages + 1;
                Models.Data.Image newImage = null;

            if (HttpContext.Session.GetString("UserImageId") != null)
            {
                newUserImage = Convert.ToInt32(HttpContext.Session.GetString("UserImageId"));
            }
                else 
                {
                
                newImage = new DiplomProba1.Models.Data.Image
                {
                    IdImages = newUserImage,
                    Image1 = diplomdbContext.Images.First().Image1
                };
                diplomdbContext.Images.Add(newImage);
                Console.WriteLine(newImage.IdImages);
            }
            HttpContext.Session.Remove("UserImageId");
            HttpContext.Session.Remove("UserImage");




            diplomdbContext.Commenttexts.Add(commenttext);
                User addUser = new User
                {
                    IdUsers = diplomdbContext.Users.ToList().OrderBy(p => p.IdUsers).Last().IdUsers + 1,
                    Name = name,
                    Surname = surname,
                    Lastname = lastName,
                    Birthday = DateOnly.ParseExact(Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Email = Email,
                    Login = numer,
                    Password = password,
                    IdRole = IdRole,
                    IdCommentText = newIdCommentText,
                    Estimation = ecimation,
                    UserImg = newUserImage ,
                    DateRegistration = DateOnly.ParseExact( DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                diplomdbContext.Users.Add(addUser);
                
                diplomdbContext.SaveChanges();
                return RedirectToAction("LogIn");
            
            


        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserImageId");
            HttpContext.Session.Remove("UserImage");

            return RedirectToAction("Index", "Home");
        }


        public IActionResult UserPage(int id)
        {
            if (id != 0)
            {
                var mySessionValue = HttpContext.Session.GetString("UserId");
                diplomdbContext diplomdbContext = new diplomdbContext();
                ViewData["CountRoute"] = diplomdbContext.Userroutes.Where(p => p.IdRoutNavigation.IdUser == id).ToList().Count() + diplomdbContext.Routes.Where(p=> p.IdUser == id).ToList().Count();
                if (HttpContext.Session.GetString("UserId") != null && 
                    diplomdbContext.Userroutes.FirstOrDefault(P=>P.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId")) &&  P.IdRoutNavigation.IdUser == id) != null &&
                    diplomdbContext.Comments.FirstOrDefault(p => p.IduserLeaveReview == Convert.ToInt32(HttpContext.Session.GetString("UserId")) && p.IdUserComment == id) == null  )
                {
                    ViewData["InRoute"] = true;
                }
                List<DiplomProba1.Models.Data.Route> routesUser = diplomdbContext.Userroutes.Where(r => r.IdUser == Convert.ToInt32(mySessionValue) && r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3).Select(p => p.IdRoutNavigation).ToList();
                List<DiplomProba1.Models.Data.Route> routesThisUser = diplomdbContext.Userroutes.Where(r => r.IdUser == id && r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3).Select(p => p.IdRoutNavigation).ToList();
                Console.WriteLine("совместные поездки "  +(routesUser.Intersect(routesThisUser)).Count());
                if (id != Convert.ToInt32(mySessionValue) &&
                    (routesUser.Intersect(routesThisUser)).Count() != 0 &&
                    diplomdbContext.Comments.FirstOrDefault(p => p.IduserLeaveReview == Convert.ToInt32(mySessionValue) && p.IdUserComment == id) == null
                    
                    ) 
                {
                    ViewData["InRoute"] = true;
                }
                    


                ViewBag.Comments = diplomdbContext.Comments.Where(c => c.IdUserComment == id).ToList();
                ViewBag.CountComments = diplomdbContext.Comments.Where(c => c.IdUserComment == id).ToList().Count();
                ViewBag.UserInfo = (DiplomProba1.Models.Data.User)diplomdbContext.Users.First(c => c.IdUsers == id);
                
            }



            return View();
        }


        public async Task<IActionResult> EditingProfile( //редактирование
        int id,
        string name,
        string surname,
        string lastName,
        string Birthday,
        string Email,
        string password,
        string numer,
        int IdCommentAbout,
        string commentAbout

        )
        {

            diplomdbContext diplomdbContext = new diplomdbContext();
            //Console.WriteLine("Name file: " + testImagecs.UserImg.Name.ToString());
            Commenttext commenttext = diplomdbContext.Commenttexts.First(c => c.IdCommentText == IdCommentAbout);
            commenttext.Text = commentAbout;
            User user = diplomdbContext.Users.First(c => c.IdUsers == id);
            diplomdbContext.Commenttexts.Add(commenttext);


            user.Name = name;
            user.Surname = surname;
            user.Lastname = lastName;
            user.Birthday = DateOnly.ParseExact(Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            user.Email = Email;
            user.Login = numer;
            user.Password = password;

            

            //if (select != null)
            //{
            //    using (var stream = new MemoryStream())
            //    {
            //        await select.CopyToAsync(stream);
            //        user.UserImgNavigation.Image1 = stream.ToArray();
            //    }
            //}

            diplomdbContext.Update(user);
            diplomdbContext.Update(commenttext);
            await diplomdbContext.SaveChangesAsync();




            return RedirectToAction("UserProfile");
        }



        public async Task<IActionResult> AddReviews(string commenttext , int estimate , int id)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            int newIdCommentText = diplomdbContext.Commenttexts.ToList().OrderBy(p => p.IdCommentText).Last().IdCommentText + 1;

            Commenttext commenttext1 = new Commenttext
            {
                IdCommentText = newIdCommentText,
                Text = commenttext
            };

            diplomdbContext.Comments.Add(new Comment
            {
                IdComment = diplomdbContext.Comments.ToList().OrderBy(p=>p.IdComment).Last().IdComment + 1,
                IduserLeaveReview = Convert.ToInt32(HttpContext.Session.GetString("UserId")),
                IdUserComment = id,
                Estimation = estimate,
                IdCommentText = newIdCommentText,
                Date = DateOnly.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture)
            });

            diplomdbContext.Commenttexts.Add(commenttext1);
            await diplomdbContext.SaveChangesAsync();
            return RedirectToAction("UserPage", new {id = id });
        }

        public RedirectToActionResult BecomeDriver()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("UserCar" , "Car");
            }
            return RedirectToAction("Aauthorization");
        }

        public RedirectToActionResult BecomeRoute()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                diplomdbContext diplomdbContext = new diplomdbContext();
                if (diplomdbContext.Users.First(p=>p.IdUsers ==Convert.ToInt32(HttpContext.Session.GetString("UserId"))).IdRole == 2  )
                {
                    return RedirectToAction("NewRoute", "Route");
                }
                return RedirectToAction("UserCar", "Car");

            }
            return RedirectToAction("Aauthorization");
        }

        public IActionResult UserCropImages()
        {
            return View();
        }

        public IActionResult UserCropImages1(string filename, IFormFile blob)
        {
            Console.WriteLine(filename);
            //try
            //{
                if (blob != null)
                {
                    diplomdbContext diplomdbContext = new diplomdbContext();
                    using (var stream = new MemoryStream())
                    {
                        blob.CopyToAsync(stream);
                        int idImag = diplomdbContext.Images.OrderBy(p=>p.IdImages).Last().IdImages + 1;
                        if (HttpContext.Session.GetString("UserImageId") != null)
                        {
                            idImag = Convert.ToInt32(HttpContext.Session.GetString("UserImageId"));
                            DiplomProba1.Models.Data.Image img = diplomdbContext.Images.First(p => p.IdImages == idImag);
                            img.Image1 = stream.ToArray();
                            diplomdbContext.Update(img);
                        }
                        else
                        {
                            diplomdbContext.Add(new DiplomProba1.Models.Data.Image
                            {
                                IdImages = idImag,
                                Image1 = stream.ToArray()
                            });
                        }
                        

                        
                        diplomdbContext.SaveChanges();

                        //HttpContext.Response.Cookies.Append("name", System.Convert.ToBase64String(stream.ToArray()));

                        HttpContext.Session.SetString("UserImage", System.Convert.ToBase64String(stream.ToArray()));
                        HttpContext.Session.SetString("UserImageId", idImag.ToString());

                        var mySessionValue = HttpContext.Session.GetString("UserImage");
                        Console.WriteLine("aaa");
                        Console.WriteLine(mySessionValue);


                        //Console.WriteLine(System.Convert.ToBase64String(stream.ToArray()));
                        Console.WriteLine("aaa");
                        

                    }; 
                }
                return Json(new { Message = "OK" });
        //}
        //    catch (Exception)
        //    {
        //        return Json(new { Message = "ERROR" });
        //    }
        }

        public IActionResult MessagePage()
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            var mySessionValue = HttpContext.Session.GetString("UserId");
            ViewBag.FutereRoute = diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 1 && r.IdUser == Convert.ToInt32(mySessionValue)).ToList();
            int a = diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 1 && r.IdUser == Convert.ToInt32(mySessionValue)).ToList().Count();
            //все маршруты пользоавтеля
            List<DiplomProba1.Models.Data.Route> routes =diplomdbContext.Userroutes.Where(r =>r.IdUser == Convert.ToInt32(mySessionValue) && r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3 &&  r.IdRoutNavigation.DataTimeStart.AddDays(1) > DateTime.Now).Select(p => p.IdRoutNavigation).ToList();

            List<User> users2 = new List<User>();
            foreach (DiplomProba1.Models.Data.Route ro in routes)
            {
                foreach (User us in ro.AllUsersGetRoute())
                {
                    Console.WriteLine("количество " + ro.AllUsersGetRoute().Count());
                    Console.WriteLine(" номер " + us.IdUsers);
                    if (diplomdbContext.Comments.FirstOrDefault(p=>p.IdUserComment == us.IdUsers && p.IduserLeaveReview == Convert.ToInt32(mySessionValue) && p.IdUserComment != Convert.ToInt32(mySessionValue)) == null )
                    {
                        Console.WriteLine("польщоавтель номер " + us.IdUsers);
                        if (us.IdUsers != Convert.ToInt32(mySessionValue))
                        {
                            users2.Add(us);
                        }
                    }
                }
            }
            
            Console.WriteLine("user " + users2.Count());
            users2 = users2.GroupBy(x => x.IdUsers).Select(y => y.First()).ToList();
            a += users2.Count();
            ViewBag.RequestUser = users2;
           

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
            a += route.Count();
            Console.WriteLine("кол сообщений " + a);
            ViewBag.VRoutes = route.ToList();
            Console.WriteLine("route " + route.Count());
            return View();
        }


        //public  void CountMess()
        //{
        //    var mySessionValue = HttpContext.Session.GetString("UserId");
        //    diplomdbContext diplomdbContext = new diplomdbContext();
        //    int CountMess = diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 1 && r.IdUser == Convert.ToInt32(mySessionValue)).Count();
        //    Console.WriteLine("предстоящие поездки" + CountMess);
        //    CountMess += diplomdbContext.Userroutes.Where(r => r.StatusUserRouteId == 1 && r.IdRoutNavigation.IdStatusRoute == 3 && r.IdUser == Convert.ToInt32(mySessionValue) && r.IdRoutNavigation.DataTimeStart.AddDays(1) < DateTime.Now).Count();
        //    Console.WriteLine("нидавние поездки" + CountMess);
        //    if (Convert.ToInt32(mySessionValue) == 2)
        //    {
        //        List<DiplomProba1.Models.Data.Route> routesVodit = diplomdbContext.Routes.Where(p => p.IdUser == Convert.ToInt32(mySessionValue) && p.IdStatusRoute == 1).ToList();
        //        CountMess += diplomdbContext.Userroutes.Where(m => m.StatusUserRouteId == 3 && routesVodit.Contains(m.IdRoutNavigation)).Count();
        //        Console.WriteLine("колчиесвсто заявок" + CountMess);
        //    }
        //    Console.WriteLine("всего" + CountMess);
        //    HttpContext.Session.SetString("CountMess", CountMess.ToString());
        //}


    }
}
