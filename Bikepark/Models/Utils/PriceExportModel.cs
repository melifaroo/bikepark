using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class PriceExportModel
    {
        [Display(Name = "#")]
        public int? PricingID { get; set; }

        [Display(Name = "Billing Plan")]
        public string? PricingName { get; set; }

        [Display(Name = "Pricing Category")]
        public string? PricingCategoryName { get; set; }

        [Display(Name = "Billing Model")]
        public string? PricingType { get; set; } 

        [Display(Name = "Days Of Week")]
        public string? DaysOfWeek { get; set; } 
        
        [Display(Name = "Holiday")]
        public string IsHoliday { get; set; } ="False";

        [Display(Name = "Concessional")]
        public string IsReduced { get; set; } = "False";

        [Display(Name = "Minimal Rental Period (hours)")]
        public int MinDuration { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}
