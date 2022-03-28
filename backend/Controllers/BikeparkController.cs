using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bikepark.Data;
using Bikepark.Models;
using Microsoft.EntityFrameworkCore;

namespace Bikepark.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class BikeparkController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BikeParkDbContext db;
        
        [BindProperty]
        public Item? FilterPattern { get; set; }


        public BikeparkController(ILogger<WeatherForecastController> logger, BikeParkDbContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Storage()
        {
            var storage = db.Storage.Include(item => item.Category).Include(item => item.SubCategory);
            return View(await storage.ToListAsync());
        }
             
        [HttpPost]
        public async Task<IActionResult> Filter( int? CategoryID )
        {
            var storage = db.Storage;
            if (CategoryID != null)
            {
                storage.Where(x => x.CategoryID == CategoryID);      
            }
            return View("Storage", await storage.ToListAsync());
        }

    }
}