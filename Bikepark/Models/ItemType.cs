using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemType 
    {
        public int ItemTypeID { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string? ItemTypeName { get; set; }
        public int? ItemCategoryID { get; set; }
        [Display(Name = "Тип")]
        public virtual ItemCategory? ItemCategory { get; set; }
        public int? PricingCategoryID { get; set; }
        [Display(Name = "Категория тарифа")]
        public virtual PricingCategory? PricingCategory { get; set; }
        [Display(Name = "Возраст")]
        public ItemAge? ItemAge { get; set; }
        [Display(Name = "Пол")]
        public ItemGender? ItemGender { get; set; }
        [Display(Name = "Размер")]
        public ItemSize? ItemSize { get; set; }
        [Display(Name = "Колесо")]
        public string? ItemWheelSize { get; set; }
        [Display(Name = "Цвет")]
        public string? ItemColor { get; set; }
        [Display(Name = "Описание")]
        public string? ItemDescription { get; set; }
        [Display(Name = "Ссылка")]
        public string? ItemExternalURL { get; set; }
        [Display(Name = "Фото")]
        public string? ItemImageURL { get; set; }

    }

}