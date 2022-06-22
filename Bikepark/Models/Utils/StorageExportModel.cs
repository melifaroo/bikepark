using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class StorageExportModel
    {
        [Display(Name = "#")]
        public int? ItemID { get; set; }

        [Display(Name = "Номер")]
        public string? ItemNumber { get; set; }

        [Display(Name = "Категория ТС")]
        public string? ItemCategoryName { get; set; }

        [Display(Name = "Модель")]
        public string? ItemTypeName { get; set; }

        [Display(Name = "Категория тарифов")]
        public string? PricingCategoryName { get; set; }

        [Display(Name = "Возраст")]
        public string? ItemAge { get; set; }
        [Display(Name = "Пол")]
        public string? ItemGender { get; set; }
        [Display(Name = "Размер")]
        public string? ItemSize { get; set; }
        [Display(Name = "Колесо")]
        public string? ItemWheelSize { get; set; }
        [Display(Name = "Цвет")]
        public string? ItemColor { get; set; }
        [Display(Name = "Описание")]
        public string? ItemDescription { get; set; }
        [Display(Name = "Ссылка")]
        public string? ItemExternalURL { get; set; }
    }
}
