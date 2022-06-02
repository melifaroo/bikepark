using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class ItemRecord : RecordInfo
    {
        public int? ItemRecordID { get; set; }
        public int? RecordID { get; set; }

        [Display(Name = "Запись о прокате")]
        public virtual Record? Record { get; set; }
        public int? ItemID { get; set; }
        [Display(Name = "Наименование")]
        public virtual Item? Item { get; set; }
        public int? PricingID { get; set; }
        [Display(Name = "Тариф")]
        public virtual Pricing? Pricing { get; set; }



    }
}
