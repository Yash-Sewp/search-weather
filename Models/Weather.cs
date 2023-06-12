using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Search_Weather.Models
{
    public class Weather
    {
        public int WeatherID { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string CityName { get; set; }

        public int ZipCode { get; set; }

        public object CityData { get; set; }

        public decimal Temp { get; set; }

    }
}