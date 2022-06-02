using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Pricing
    {
        public int? PricingID { get; set; }
        [Display(Name = "Тариф")]
        public string? PricingName { get; set; }
        [Display(Name = "Категория тарифа")]
        public int? PricingCategoryID { get; set; }
        [Display(Name = "Категория тарифа")]
        public virtual PricingCategory? PricingCategory { get; set; }
        [Display(Name = "Тарификация")]
        public PricingType PricingType { get; set; } = PricingType.Hourly;
        [Display(Name = "Дни недели")]
        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek> {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday,
            };
        [Display(Name = "Праздничный")]
        public Boolean IsHoliday { get; set; }
        [Display(Name = "Льготный")]
        public Boolean IsReduced { get; set; }
        [Display(Name = "Минимум времени")]
        public int MinDuration { get; set; } = 1;
        [Display(Name = "Цена")]
        public double Price { get; set; } = 1;
        
        public bool Archival { get; set; } = false;
    }
}
