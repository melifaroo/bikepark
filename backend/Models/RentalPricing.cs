using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class RentalPricing
    {
        public int? RentalPricingID { get; set; }
        [Display(Name = "Тариф")]
        public string? RentalPricingName { get; set; }
        public int PricingCategoryID { get; set; }
        [Display(Name = "Категория тарифа")]
        public virtual PricingCategory? PricingCategory { get; set; }
        [Display(Name = "Тарификация")]
        public RentalType RentalType { get; set; } = RentalType.Hourly;
        [Display(Name = "Дни недели")]
        public List<DayOfWeekRu> DaysOfWeek { get; set; } = new List<DayOfWeekRu>();
        [Display(Name = "Праздничный")]
        public Boolean IsHoliday { get; set; }
        [Display(Name = "Льготный")]
        public Boolean IsReduced { get; set; }
        [Display(Name = "Минимум времени")]
        public int MinDuration { get; set; } = 1;
        [Display(Name = "Цена")]
        public double Price { get; set; } = 1;
        [Display(Name = "Цена2" )]
        public double ExtraPrice { get; set; } = 1;

    }
}
