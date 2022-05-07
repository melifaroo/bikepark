using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class RentalPricing
    {
        public int? RentalPricingID { get; set; }
        [Display(Name = "Тариф")]
        public string? RentalPricingName { get; set; }
        public int ItemCategoryID { get; set; }
        [Display(Name = "Категория")]
        public virtual ItemCategory? ItemCategory { get; set; }
        [Display(Name = "Почасовой/суточный")]
        public RentalType RentalType { get; set; } = RentalType.Hourly;
        [Display(Name = "Дни недели")]
        public List<DayOfWeekRu> DaysOfWeek { get; set; } = new List<DayOfWeekRu>();
        [Display(Name = "Праздничный")]
        public Boolean IsHoliday { get; set; }
        [Display(Name = "Льготный")]
        public Boolean IsReduced { get; set; }
        [Display(Name = "Ставка")]
        public double Price { get; set; } = 1;

    }
}
