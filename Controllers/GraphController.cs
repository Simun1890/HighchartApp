using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using YourApp.Models;

namespace YourApp.Controllers
{
    public class GraphController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public GraphController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var folderPath = Path.Combine(_env.WebRootPath, "Data");
            var files = Directory.GetFiles(folderPath, "*.csv");

            var allCharts = new List<CijenaPodaci>();

            foreach (var file in files)
            {
                var (proizvod, podaci) = ParseCsv(file);

                allCharts.Add(new CijenaPodaci
                {
                    Proizvod = proizvod,
                    Podaci = podaci
                });
            }

            return View(allCharts);
        }

        private (string nazivProizvoda, List<DataPoint>) ParseCsv(string path)
        {
            var result = new List<DataPoint>();
            var lines = System.IO.File.ReadAllLines(path).ToList();

            if (lines.Count < 2)
                return ("Nepoznat proizvod", result); // fallback ako je datoteka prazna

            var firstDataLine = lines[1];
            var parts = firstDataLine.Split(';');
            string nazivProizvoda = parts[0]; // prvi stupac = naziv proizvoda

            foreach (var line in lines.Skip(1)) // preskoči header
            {
                var fields = line.Split(';');
                if (fields.Length >= 5 &&
                    DateTime.TryParseExact(fields[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum) &&
                    double.TryParse(fields[4].Replace(" €", "").Replace(" kn", "").Replace(',', '.'),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out double cijena))
                {
                    result.Add(new DataPoint
                    {
                        Datum = datum,
                        Cijena = cijena
                    });
                }
            }

            return (nazivProizvoda, result);
        }
    }
}