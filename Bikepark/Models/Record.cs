using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Record : RecordInfo
    {
        public int? RecordID { get; set; }
        public int? CustomerID  { get; set; }
        [Display(Name = "Клиент")]
        public virtual Customer? Customer { get; set; }

        [InverseProperty("Record")]        
        public virtual List<ItemRecord> ItemRecords { get; set; } = new List<ItemRecord>();

        [Display(Name = "Стоимость")]
        public double? Price { get; set; }
        
    }
}
