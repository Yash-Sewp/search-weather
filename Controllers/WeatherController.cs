using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;
using Search_Weather.Model;
using Search_Weather.Models;
using System.Net.Http.Headers;

namespace Search_Weather.Controllers
{
    public class WeatherController : Controller
    {

        static HttpClient client = new HttpClient();
        private Search_WeatherContext db = new Search_WeatherContext();

        Weather[] cities = new Weather[]
        {
            new Weather { WeatherID = 1, CityName = "benoni", ZipCode =  1501, CityData = new { latitude = -26.15, longitude = 28.36 } },
            new Weather { WeatherID = 2, CityName = "boksburg", ZipCode =  1459, CityData = new { latitude = -26.23, longitude = 28.24 } },
            new Weather { WeatherID = 3, CityName = "brakpan", ZipCode =  1459, CityData = new { latitude = -26.29, longitude = 28.32 } },
            new Weather { WeatherID = 4, CityName = "carletonville", ZipCode =  2495, CityData = new { latitude = -26.35, longitude = 27.39 } },
            new Weather { WeatherID = 5, CityName = "germiston", ZipCode =  1440, CityData = new { latitude = -26.21, longitude = 28.16 } },
            new Weather { WeatherID = 6, CityName = "johannesburg", ZipCode =  1709, CityData = new { latitude = -26.19, longitude = 28.03 } },
            new Weather { WeatherID = 7, CityName = "krugersdorp", ZipCode =  1739, CityData = new { latitude = -26.09, longitude = 27.80 } },
            new Weather { WeatherID = 8, CityName = "pretoria", ZipCode =  0118, CityData = new { latitude = -25.73, longitude = 28.21 } },
            new Weather { WeatherID = 9, CityName = "randburg", ZipCode =  2194, CityData = new { latitude = -26.14, longitude = 27.99 } },
            new Weather { WeatherID = 10, CityName = "randfontein", ZipCode =  1759, CityData = new { latitude = -26.19, longitude = 27.67 } },
            new Weather { WeatherID = 11, CityName = "roodepoort", ZipCode =  1724, CityData = new { latitude = -26.12, longitude = 27.90 } },
            new Weather { WeatherID = 12, CityName = "soweto", ZipCode =  1723, CityData = new { latitude = -26.26, longitude = 27.86 } },
            new Weather { WeatherID = 13, CityName = "springs", ZipCode =  1496, CityData = new { latitude = -26.25, longitude = 28.44 } },
            new Weather { WeatherID = 14, CityName = "vanderbijlpark", ZipCode =  1900, CityData = new { latitude = -26.70, longitude = 27.80 } },
            new Weather { WeatherID = 15, CityName = "vereeniging", ZipCode =  1805, CityData = new { latitude = -26.67, longitude = 27.93 } },
        };


        // GET: Weather
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        [HttpPost]
                [ValidateAntiForgeryToken]
        public ActionResult GetWeather(string cityName)
        {
        
            Session["Data"] = null;

            var data = cities.Where(c => c.CityName == cityName.ToLower()).ToList();

            if (data == null)
            {
                ViewBag.error = "City does not exist in database.";
            }

            Session["Data"] = data.FirstOrDefault();

            TempData["CityDetails"] = data.FirstOrDefault();

            return RedirectToAction("Results");


        }

        string Baseurl = "https://api.openweathermap.org/";
        public async Task<ActionResult> Results()
        {

            Weather EmpInfo = new Weather();

            dynamic TempCityDetails = Session["Data"];
            dynamic weatherInfo;

            EmpInfo = TempCityDetails as Weather;


            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (TempCityDetails == null) {
                return RedirectToAction("Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"data/2.5/weather?lat={TempCityDetails.CityData.latitude}&lon={TempCityDetails.CityData.longitude}&appid=19d7d13a33c209d11f6e2346665f97bc&units=metric");

                if (Res.IsSuccessStatusCode)
                {
                    var apiContent = await Res.Content.ReadAsStringAsync();
                    weatherInfo = JsonConvert.DeserializeObject<dynamic>(apiContent);

                    EmpInfo.Temp = weatherInfo.main.temp;
                }
                
                return View(EmpInfo);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
