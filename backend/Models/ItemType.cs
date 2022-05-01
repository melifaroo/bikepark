using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemType 
    {
        public int ItemTypeID { get; set; }
        [Required]
        [Display(Name = "������������")]
        public string? ItemTypeName { get; set; }                
        public int? ItemCategoryID {get; set;}
        [Display(Name = "���������")]
        public virtual ItemCategory? ItemCategory {get; set;}
        [Display(Name = "���")]
        public ItemAge? ItemAge { get; set; }
        [Display(Name = "���")]
        public ItemGender? ItemGender { get; set; }
        [Display(Name = "������/��������")]
        public ItemSize? ItemSize { get; set; }
        [Display(Name = "������ ������")]
        public string? ItemWheelSize { get; set; }
        [Display(Name = "������ ������")]
        public string? ItemColor { get; set; }
        [Display(Name = "��������")]
        public string? ItemDescription { get; set; }
        [Display(Name = "������")]
        public string? ItemExternalURL { get; set; }
        [Display(Name = "����")]
        public string? ItemImageURL { get; set; }

    }

}