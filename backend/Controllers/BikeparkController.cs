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
        private readonly BikeparkDbContext db;


        public BikeparkController(ILogger<WeatherForecastController> logger, BikeparkDbContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Storage()
        {
            var storage = db.Storage.Include(x => x.Category).Include(x => x.SubCategory);
            return View(await storage.ToListAsync());
        }
             
        [HttpPost]
        public async Task<IActionResult> Filter([FromForm] Item filter )
        {
            var filteredStorage = db.Storage.Include(x => x.Category).Include(x => x.SubCategory).Select(x => x);
            if (filter.CategoryID != -1)
                filteredStorage = filteredStorage.Where(x => x.CategoryID == filter.CategoryID);
            if (filter.SubCategoryID != -1)
                filteredStorage = filteredStorage.Where(x => x.SubCategoryID == filter.SubCategoryID);
            if (filter.Name != null)
                filteredStorage = filteredStorage.Where(x => x.Name == filter.Name);
            if (filter.Size != -1)
                filteredStorage = filteredStorage.Where(x => x.Size == filter.Size);
            if (filter.Number != -1)
                filteredStorage = filteredStorage.Where(x => x.Number == filter.Number);
            if (filter.Status != null)
                filteredStorage = filteredStorage.Where(x => x.Status == filter.Status);

            return View("Storage", await filteredStorage.ToListAsync());
        }

    }
}