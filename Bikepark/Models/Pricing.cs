using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Pricing
    {
        public int? PricingID { get; set; }

        [Display(Name = "Billing Plan")]
        public string? PricingName { get; set; }

        [Display(Name = "Pricing Category")]
        public int? PricingCategoryID { get; set; }

        [Display(Name = "Pricing Category")]
        public virtual PricingCategory? PricingCategory { get; set; }

        [Display(Name = "Billing Model")]
        public PricingType PricingType { get; set; } = PricingType.Hourly;

        [Display(Name = "Days Of Week")]
        public DaysOfWeekFlags DaysOfWeek { get; set; } = DaysOfWeekFlags.AllDays;
        // public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek> {
        //             DayOfWeek.Monday,
        //             DayOfWeek.Tuesday,
        //             DayOfWeek.Wednesday,
        //             DayOfWeek.Thursday,
        //             DayOfWeek.Friday,
        //             DayOfWeek.Saturday,
        //             DayOfWeek.Sunday,
        //     };

        [Display(Name = "Holiday")]
        public Boolean IsHoliday { get; set; }

        [Display(Name = "Concessional")]
        public Boolean IsReduced { get; set; }

        [Display(Name = "Minimal Rental Period (hours)")]
        public int MinDuration { get; set; } = 1;

        [Display(Name = "Price")]
        public double Price { get; set; } = 1;
                
        public bool Archival { get; set; } = false;
    }
}
