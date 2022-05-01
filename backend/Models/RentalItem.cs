using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class RentalItem : RentalInfo
    {
        public int RentalItemID { get; set; }
        public int RentalRecordID { get; set; }
        [Display(Name = "Запись о прокате")]
        public virtual RentalRecord? RentalRecord { get; set; }
        public int? ItemID { get; set; }
        [Display(Name = "Наименование")]
        public virtual Item? Item { get; set; }
        public int? RentalPricingID { get; set; }
        [Display(Name = "Тариф")]
        public virtual RentalPricing? RentalPricing { get; set; }

        public bool IsPaid { get; set; } = false;
    }
}
