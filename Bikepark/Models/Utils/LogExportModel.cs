using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class LogExportModel
    {
        [Display(Name = "#")]
        public int? ItemRecordID { get; set; }

        [Display(Name = "Категория ТС")]
        public string? ItemCategoryName { get; set; }

        [Display(Name = "Модель")]
        public string? ItemTypeName { get; set; }

        [Display(Name = "Номер")]
        public string? ItemNumber { get; set; }

        [Display(Name = "Начало проката")]
        public DateTime? Start { get; set; }

        [Display(Name = "Конец проката")]
        public DateTime? End { get; set; }

        [Display(Name = "Статус")]
        public string? Status { get; set; }

        [Display(Name = "Тариф")]
        public string? PricingName { get; set; }

        [Display(Name = "Цена")]
        public double? Price { get; set; }

        [Display(Name = "Тарификация")]
        public string? PricingType { get; set; }

        [Display(Name = "#Заказ")]
        public int? RecordID { get; set; }
    }
}
