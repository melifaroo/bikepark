using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class ItemRecord : RecordInfo
    {
        public int? ItemRecordID { get; set; }

        public int? RecordID { get; set; }

        [Display(Name = "Order Record")]
        public virtual Record? Record { get; set; }

        public int? ItemID { get; set; }

        [Display(Name = "Item")]
        public virtual Item? Item { get; set; }

        public int? PricingID { get; set; }

        [Display(Name = "Billing Plan")]
        public virtual Pricing? Pricing { get; set; }



    }
}
