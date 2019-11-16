using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SPA_angular_test.Controllers
{   
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        // My Code Below

        [HttpGet("[action]")]
        public bool AddImages(string directory)
        {
            DirectoryManipulation directorytotest = new DirectoryManipulation(directory);
            return directorytotest.valid;
        }

        [HttpGet("[action]")]
        public string[][] GetImages()
        {
            return Thumbnail_Retrieval.Retrieve_Thumbnail();
        }


        public class DirectoryManipulation
        {
            public string Dir { get; set; }
            public Boolean valid { get; set; }

            public DirectoryManipulation(string Dir)
            {
                this.Dir = Dir;
                valid = Directory.Exists(Dir);

                try
                {
                    addDirectory();

                } catch(Exception E)
                {
                    System.Diagnostics.Debug.WriteLine(E);
                    this.valid = false;
                }
            }

            public void addDirectory()
            {
                if (valid)
                {
                    Thumbnail_Saving.Scanner(this.Dir);
                }
            }
        }        
    }
}
