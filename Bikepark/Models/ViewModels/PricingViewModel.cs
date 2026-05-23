using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models.ViewModels
{
    public class PricingViewModel
    {
        public int? PricingID { get; set; }

        [Display(Name = "Billing Plan")]
        public string? PricingName { get; set; }

        [Display(Name = "Pricing Category")]
        public int? PricingCategoryID { get; set; }

        [Display(Name = "Pricing Category")]
        public virtual PricingCategoryViewModel? PricingCategory { get; set; }

        [Display(Name = "Billing Model")]
        public PricingType PricingType { get; set; }

        [Display(Name = "Days Of Week")]
        public DaysOfWeekFlags DaysOfWeek { get; set; }
        // public List<DayOfWeek> DaysOfWeek { get; set; } = new();

        [Display(Name = "Holiday")]
        public Boolean IsHoliday { get; set; }

        [Display(Name = "Concessional")]
        public Boolean IsReduced { get; set; }

        [Display(Name = "Minimal Rental Period (hours)")]
        public int MinDuration { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }
                
        public bool Archival { get; set; }
    }
}