using Microsoft.EntityFrameworkCore;

namespace Bikepark.Models
{
    public class PricingFilter
    {

        public static async Task<List<Pricing>> ActualPricing(DbSet<Pricing> pricing, int? PricingCategoryID, DateTime start, DateTime end, bool isHoliday)//DayOfWeek dayOfWeek
        {
            var Duration = Math.Ceiling(end.Subtract(start).TotalHours);
            return await pricing
                .FromSqlRaw($"SELECT * FROM 'Pricings' WHERE DaysOfWeek LIKE '%{start.DayOfWeek}%'")
                //.Where(x => !x.Archival)
                .Where(price => 
                ((price.PricingCategoryID == null || PricingCategoryID == null) ? true : price.PricingCategoryID == PricingCategoryID) && 
                price.PricingType < PricingType.Service && 
                price.MinDuration <= Duration && (!price.IsHoliday || isHoliday )).ToListAsync();
        }

        public static async Task<List<Pricing>> ServicePricing(DbSet<Pricing> pricing, int? PricingCategoryID)
        {
            return await pricing//.Where(x => !x.Archival)
                .Where(price => price.PricingType == PricingType.Service && ( price.PricingCategoryID == PricingCategoryID || price.PricingCategoryID == null )).ToListAsync();
        }


    }
}
