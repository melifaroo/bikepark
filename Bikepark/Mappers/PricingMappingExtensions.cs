using Bikepark.Models;
using Bikepark.Models.ViewModels;

namespace Bikepark.Mappers
{
    
    public static class PricingMappingExtensions
    {
        public static PricingCategoryViewModel ToViewModel( this PricingCategory pricingCategory )
        {
            return new PricingCategoryViewModel
            {
                PricingCategoryID = pricingCategory.PricingCategoryID,
                PricingCategoryName = pricingCategory.PricingCategoryName
            };            
        }

        public static PricingViewModel ToViewModel( this Pricing pricing )
        {
            return new PricingViewModel
            {
                PricingID = pricing.PricingID,
                PricingName = pricing.PricingName,
                PricingCategoryID = pricing.PricingCategoryID,
                PricingCategory = pricing.PricingCategory?.ToViewModel(),
                PricingType = pricing.PricingType,
                DaysOfWeek = pricing.DaysOfWeek,
                IsHoliday = pricing.IsHoliday,
                IsReduced = pricing.IsReduced,
                MinDuration= pricing.MinDuration,
                Price = pricing.Price,
                Archival = pricing.Archival
            };
        }
    }
}
