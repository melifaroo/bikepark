using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class PriceExportModel
    {
        [Display(Name = "#")]
        public int? PricingID { get; set; }

        [Display(Name = "Тариф")]
        public string? PricingName { get; set; }
        [Display(Name = "Категория тарифа")]
        public string? PricingCategoryName { get; set; }
        [Display(Name = "Тарификация")]
        public string? PricingType { get; set; } 
        [Display(Name = "Дни недели")]
        public string? DaysOfWeek { get; set; } 
        [Display(Name = "Праздничный")]
        public string IsHoliday { get; set; }
        [Display(Name = "Льготный")]
        public string IsReduced { get; set; }
        [Display(Name = "Минимум времени проката (часов)")]
        public int MinDuration { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
    }
}
