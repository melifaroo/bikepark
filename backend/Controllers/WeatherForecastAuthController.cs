using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bikepark.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WeatherForecastAuthController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastAuthController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return Data().ToArray();
        }

        public IActionResult List()
        {
            return View(Data().ToList());
        }

        private IEnumerable<WeatherForecast> Data()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }
        

    }
}