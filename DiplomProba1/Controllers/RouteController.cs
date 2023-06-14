using Microsoft.AspNetCore.Mvc;
using DiplomProba1.Models.Data;
using System.Globalization;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace DiplomProba1.Controllers
{
    public class RouteController : BaseController
    {
        private readonly IConverter _converter;

        public RouteController( IConverter converter)
        {
            _converter= converter;
        }

        public IActionResult FoundRoutes(string BeginRoute, string EndRoute, string date , string all ,int? mincost , int? maxcost) //поиск маршрутов
        {
            Console.WriteLine(BeginRoute + " : " + EndRoute);
            ViewBag.Maxcost = 20000;
            ViewBag.Mincost = 0;

            ViewBag.DateNow = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime date1 = date != null ? Convert.ToDateTime(date) : DateTime.Now;
            
            diplomdbContext diplomdbContext = new diplomdbContext();
            List<Models.Data.Route> route = diplomdbContext.Routes.Where(p=>p.IdStatusRoute == 1).ToList();
            if (BeginRoute != null)
            {
                ViewBag.BeginRoute = BeginRoute;
                route = route.Where(r => r.BeginRoute.Trim() == BeginRoute.Trim()).ToList();
            }
            if (EndRoute != null)
            {
                ViewBag.EndRoute = EndRoute;
                route = route.Where(r => r.EndRoute.Trim() == EndRoute.Trim()).ToList();
                route = route.OrderBy(p => Math.Abs((date1 - p.DataTimeStart.Date).TotalSeconds)).ToList();
            }
            
            if (date != null)
            {
                ViewBag.DateRoute = date;
            }
            if (all != null)
            {
                ViewBag.all = all;
                route = route.Where(r => r.CountFreeRoute() >= Convert.ToInt32(all)).ToList();
                
            }
            route = route.OrderBy(p => Math.Abs((date1 - p.DataTimeStart.Date).TotalSeconds)).ToList();
            if (mincost != null)
            {
                route = route.Where(r => r.Cost >= mincost).ToList();
                ViewBag.Mincost = mincost;
            }
            if (maxcost != null)
            {
                route = route.Where(r => r.Cost <= maxcost).ToList();
                ViewBag.Maxcost = maxcost;
            }
            if (route.Count() != 0)
            {
                
                ViewBag.routes = route;
            }
            
            return View();
        }


        

        public IActionResult RoutePage(int id ,int count)
        {
            ViewBag.count= count;
            diplomdbContext diplomdbContext = new diplomdbContext();
            DiplomProba1.Models.Data.Route route = diplomdbContext.Routes.First(p => p.IdRout == id);
            ViewBag.Routeinfo = route;
            User us = diplomdbContext.Routes.First(p => p.IdRout == id).IdUserNavigation;
            ViewBag.Comments = diplomdbContext.Comments.Where(c => c.IdUserComment == us.IdUsers).ToList();
            ViewBag.CountComments = diplomdbContext.Comments.Where(c => c.IdUserComment == us.IdUsers).ToList().Count();
            ViewBag.UserInfo = us;

            if (route.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId")))
            {
                ViewData["UserChekInRoute"] = "Vodetel";
                //DiplomProba1.Models.Data.Route neerRoute = diplomdbContext.Routes.Where(p => p.IdStatusRoute != 3 && us.IdUsers == Convert.ToInt32(HttpContext.Session.GetString("UserId"))).OrderBy(p => p.DataTimeStart).FirstOrDefault();
                
                if (route != null)
                {
                    Console.WriteLine("номер рейса"+ route.IdRout);
                    Console.WriteLine("дата рейса" + route.DataTimeStart);
                    if (route.DataTimeStart > DateTime.Now)
                    {
                        ViewData["TimeRoute"] = "Вы сможете начать поездку только по достижению запланированного времени";
                    }
                    else if (route.IdStatusRoute == 1 || route.IdStatusRoute == 4)
                    {
                        ViewData["InRoute"] = "Начать поездку";
                    }
                    else if (route.IdStatusRoute == 2)
                    {
                        ViewData["InRoute"] = "Закончить поездку";
                    }
                }
            }
            else if (diplomdbContext.Userroutes.Where(m => m.IdRout == id && m.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId"))).FirstOrDefault() != null)
            {

                switch (diplomdbContext.Userroutes.Where(m => m.IdRout == id && m.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId"))).First().StatusUserRouteId)
                {
                    case 2:
                        ViewData["UserChekInRoute"] = "Ваша заявка отклонена";
                        break;
                    case 1:
                        ViewData["UserChekInRoute"] = "Ваша заявка одобрена, приходите к месту сбора в назначенное время";
                        break;
                    case 3:
                        ViewData["UserChekInRoute"] = "Вы уже подавали заявку, пожалуйста, дождитесь ответа";
                        break;
                }
                List<Userroute>  userrou = diplomdbContext.Userroutes.Where(m => m.IdRout == id && m.IdRoutNavigation.IdStatusRoute == 3 && m.IdUser == Convert.ToInt32(HttpContext.Session.GetString("UserId"))).ToList();
                
                if (userrou.Count != 0 && userrou.First().StatusUserRouteId == 1)
                {
                    ViewData["UserChekInRoute"] = "Как прошла поездка? Оставьте отзыв о водителе";
                }
            }
            ViewData["CountRoute"] = diplomdbContext.Userroutes.Where(p => p.IdRoutNavigation.IdUser == us.IdUsers).ToList().Count() + diplomdbContext.Routes.Where(p => p.IdUser == us.IdUsers).ToList().Count();

            return View();
        } //страниц промотра маршрута


        public IActionResult NewRoute() //страница добавление нового маршрута
        {
            return View();

        }

        public async Task<IActionResult> CreateNewRoute(string BeginRoute, string EndRoute, DateTime date , string time, int cost, int CountPass, string text)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            Commenttext commenttext = new Commenttext
            {
                IdCommentText = diplomdbContext.Commenttexts.OrderBy(o => o.IdCommentText).Last().IdCommentText + 1,
                Text = text
            };
            Console.WriteLine(time);
            Console.WriteLine(time.Substring(0, 2)); 
            Console.WriteLine(time.Substring(3));
            DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(time.Substring(0 ,2)), Convert.ToInt32(time.Substring(3)) , 0);
            Console.WriteLine("Time" + dateTime);
            Console.WriteLine(date);
            Models.Data.Route route = new Models.Data.Route
            {
                IdRout = diplomdbContext.Routes.OrderBy(o => o.IdRout).Last().IdRout + 1,
                IdUser = Convert.ToInt32(HttpContext.Session.GetString("UserId")),
                BeginRoute = BeginRoute,
                EndRoute = EndRoute,
                DataTimeStart = dateTime,
                Cost = cost,
                CountPassagir = CountPass,
                IdStatusRoute = 1,
                IdCommentText = commenttext.IdCommentText,
                DateAddedRoude =  DateOnly.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            diplomdbContext.Commenttexts.Add(commenttext);
            diplomdbContext.Routes.Add(route);
            await diplomdbContext.SaveChangesAsync();

            return RedirectToAction("UserProfile", "User");

        }


        public IActionResult ViewRoute(string activ)
        {
            activ= activ.Trim();
            diplomdbContext diplomdbContext = new diplomdbContext();
            var mySessionValue = HttpContext.Session.GetString("UserId");

            //List<DiplomWebsitePoputchici.Models.Data.Userroute> userroutes = diplomdbContext.Userroutes.Where(m => m.IdRoutNavigation.IdUser == Convert.ToInt32(mySessionValue)).ToList();
            List<DiplomProba1.Models.Data.Route> userroute = new List<Models.Data.Route>();
                //diplomdbContext.Routes.Where(p => p.IdStatusRoute == 1 && p.IdUser == Convert.ToInt32(mySessionValue) && p.DataTimeStart > DateTime.Now).ToList();
            Console.WriteLine("количество  " + userroute.Count());
            if (activ.ToString() == "1")
            {
                userroute = diplomdbContext.Routes.Where(p => (p.IdStatusRoute == 1 || p.IdStatusRoute == 4) && p.IdUser == Convert.ToInt32(mySessionValue)).ToList();
                Console.WriteLine("количество 1 " + userroute.Count());
            }
            else if (activ.ToString() == "3")
            {
                userroute = diplomdbContext.Routes.Where(p => p.IdStatusRoute == 3 && p.IdUser == Convert.ToInt32(mySessionValue)).ToList();
                Console.WriteLine("количество 3 " + userroute.Count());

            }
            

            //List<DiplomProba1.Models.Data.Route> olduserroute = diplomdbContext.Routes.Where(p => p.IdStatusRoute == 1 && p.IdUser == Convert.ToInt32(mySessionValue) && p.DataTimeStart > DateTime.Now).ToList();
            Console.WriteLine("количество заявок " + userroute.Count());
            ViewBag.UserRoute = userroute.OrderBy(p=>p.DataTimeStart);
            return View();
        }


        public IActionResult StatusRoute(int id ,string status)
        {
            diplomdbContext diplomdbContext = new diplomdbContext();
            if (status == "Закончить поездку")
            {
                diplomdbContext.Routes.First(p => p.IdRout == id).IdStatusRoute = 3;
            }
            else if(status == "Начать поездку")
            {
                diplomdbContext.Routes.First(p => p.IdRout == id).IdStatusRoute = 2;
            }
            diplomdbContext.SaveChanges();
            return RedirectToAction("UserProfile" , "User");
        }

        
        public IActionResult OtchetPDF(int id)
        {
            var mySessionValue = HttpContext.Session.GetString("UserId");
            diplomdbContext diplomdbContext = new diplomdbContext();
            DiplomProba1.Models.Data.Route route = diplomdbContext.Routes.First(p=>p.IdRout == id);
            Userroute userroute = diplomdbContext.Userroutes.First(p=>p.IdRout == id && p.IdUser == Convert.ToInt32(mySessionValue)) ;
            

            var html = @"
          <!DOCTYPE html>
           <html lang=""en"">
           <head>
            <style type = ""text/css"">
            .pass {
                border: 1px solid black;
                padding: 0 30px;
                margin: 0 30px;
                border-color: black;
                border-radius: 10px;
            }
             .row {
                display: -webkit-box;
                display: -ms-flexbox;
                display: flex;
                -ms-flex-wrap: wrap;
                flex-wrap: wrap;
                margin-right: -15px;
                margin-left: -15px;
            }
            .justify-content-between {
                -webkit-box-pack: justify!important;
                -ms-flex-pack: justify!important;
                justify-content: space-between!important;
            }
            .justify-content-around {
                justify-content: space-around;
            }
            .d-flex {
            display: -webkit-box!important;
            display: -ms-flexbox!important;
            display: flex!important;
            }
            .align-items-center {
                -webkit-box-align: center!important;
                -ms-flex-align: center!important;
                align-items: center!important;
            }
            </style>
               Чек от " + DateTime.Now + @"
            </head>
          <body class=""pass"">
            <header class=""row justify-content-between "">
                <img src = ""https://sun9-44.userapi.com/impg/S66hf0kg-iNPm2ryzbrQGEUAr2NmmBcfhBxQSg/z2s1pvK4MY8.jpg?size=207x82&quality=96&sign=662ce67a322160884bd1c90d3433fb17&type=album"" alt="""">
                <h3>Билет номер: " + userroute.IdUserroutes + @" 2</h3>
            </header>

            <main>
                <div class="" row justify-content-between "">
                    <div>
                        <p>Пассажир</p>
                        <h3>" + userroute.IdUserNavigation.Fulname + @"</h3>
                    </div>
                    <div>
                        <p>Водитель</p>
                        <h3>" + userroute.IdRoutNavigation.IdUserNavigation.Fulname + @"</h3>
                    </div>
                    <div>
                        <p>Номер маршрута</p>
                        <h3>" +userroute.IdRout +@"</h3>
                    </div>
                </div>

                <div>
                    <div>
                        
                        <div>
                            
                            <div class="" d-flex align-items-center justify-content-between "" style=""padding-top: 10px;"">
                                <div>
                                    <h4> Из: " + route.BeginRoute + @"
                                    <h4> В: "+ route.EndRoute + @"</h4>
                                </div>
                                <div>
                                    <h4>Дата и время: " + route.DataRoute + @"</h4>
                                    <h4>Время: " +  route.TimeRoute + @"</h4>
                                </div>
                                <div>
                                    <h4> Цена: " +route.Cost +@" ₽</h4>
                                    <h4> Мест забронировано: " + userroute.Bookcount + @"</h4>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                </div>
            </main>
          </body>
          </html>
          ";

            TempData["Message"] =  html;
            return RedirectToAction("PDF");
        }




        
        public IActionResult OtchetPDFVoditel(int id)
        {
            //var mySessionValue = HttpContext.Session.GetString("UserId");
            diplomdbContext diplomdbContext = new diplomdbContext();
            DiplomProba1.Models.Data.Route route = diplomdbContext.Routes.First(p => p.IdRout == id);
            //Userroute userroute = diplomdbContext.Userroutes.First(p => p.IdRout == id);

            string users = "";

            foreach(Userroute us in route.AllUsersGetUserRoute())
            {
                users += @"
                        <div class="" d-flex drow"" >
                            <div class=""pass wi"" >
                                <div class="" d-flex drow between"" >
                                    <div class="" d-flex drow center"" >
                                        <div>
                                                <h4> " + us.IdUserNavigation.Fulname + @"</h4>
                                            <div>
                                                <div >
                                                    <p>Забронировано мест " + us.Bookcount + @"</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="" d-flex drow center"" >
                                        <div >
                                            <p style=""padding-top: 20px;"">Номер телефона " + us.IdUserNavigation.Login + @"</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
";
            }

            var html = @"
         
<!DOCTYPE html>
<html lang=""en"">
<head>
    Чек от 23.03.2023 12:12:12
</head>
<style>
.d-flex {
            display: -webkit-box;
            display: flex!important;
            }
     .pass {
            border: 1px solid black;
            padding: 5px 30px;
            margin: 5px 30px;
                border-color: black;
                border-radius: 10px;
            }
.drow {
display: -webkit-box;
            display: flex;
            }
    
    .between {
                -webkit-box-pack: justify!important;
                -ms-flex-pack: justify!important;
                justify-content: space-between!important;
                justify-content: space-between!important;
            }
    .center {
                
                align-items: center;
            }
    .wi{
    width: 90%;
    }
    .wi1{
    width: 100%;
    }
    
</style>
<body class=""pass"">
    <header class="" d-flex drow between"">
        <img src = ""https://sun9-44.userapi.com/impg/S66hf0kg-iNPm2ryzbrQGEUAr2NmmBcfhBxQSg/z2s1pvK4MY8.jpg?size=207x82&quality=96&sign=662ce67a322160884bd1c90d3433fb17&type=album"" >
        <h3>Рейс номер 2</h3>
    </header>

    <main>
        <div>
            <div style="" padding-left: 30px;"">
                            <div>
                    <div class="" d-flex drow between wi1"" >
                        <div class="" d-flex drow  wi1"" >

                            <div class="" wi"" >
                                <div class="" d-flex drow between  wi1"" >
                                    <div>
                                        <h4>Выезд из: " + route.BeginRoute + @"</h4>
                                        <h4>В: " + route.EndRoute + @"</h4>
                                    </div>

                                </div>
                                <div class="" d-flex drow between wi"" >
                                    <div>
                                        <h4>Дата: "+ route.DataRoute+ @"</h4>
                                        <h4>Время: "+ route.TimeRoute+ @"</h4>
                                    </div>
                                    <div>
                                        <h4>Цена: "+ route.Cost+ @" Р</h4>
                                        <h4>Мест: "+route.CountPassagir + @" </h4>
                                    </div>
                                    <div>
                                        <h4>Пассажиров:"+ route.AllUsersGetRoute().Count()+ @"</h4>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>" + users + @"
                
                        
                    
            </div>
        </div>
        </div>
    </main>
</body>
</html>
          ";
            TempData["Message"] = html;
            return RedirectToAction("PDF");
        }



        [HttpGet("GeneratePDF")]
        public IActionResult PDF()
        {
            var html = TempData["Message"].ToString();
            Console.WriteLine(html);
            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = PaperKind.A4;
            //globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
            ObjectSettings objectSettings = new ObjectSettings();
            //objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            headerSettings.FontSize = 15;
            headerSettings.FontName = "Ariel";
            //headerSettings.Right = "Page [page] of [toPage]";
            headerSettings.Line = true;
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 12;
            footerSettings.FontName = "Ariel";
            //footerSettings.Center = "This is for demonstration purposes only.";
            footerSettings.Line = true;
            //objectSettings.HeaderSettings = headerSettings;
            //objectSettings.FooterSettings = footerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdfFile = _converter.Convert(htmlToPdfDocument); ;
            return File(pdfFile,
            "application/octet-stream", "DemoPdf.pdf");
        }
    }
}
