using Microsoft.EntityFrameworkCore;

namespace Bikepark.Models
{
    public class PricingFilter
    {

        public static async Task<List<Pricing>> ServicePricing(DbSet<Pricing> pricing, int? PricingCategoryID)
        {
            return await pricing//.Where(x => !x.Archival)
                .Where(price => price.PricingType == PricingType.Service && ( price.PricingCategoryID == PricingCategoryID || price.PricingCategoryID == null )).ToListAsync();
        }


    }
}
