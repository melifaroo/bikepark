using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class RentalRecord : RentalInfo
    {
        public int RentalRecordID { get; set; }
        public int? CustomerID  { get; set; }
        [Display(Name = "Клиент")]
        public virtual Customer? Customer { get; set; }
        [Display(Name = "Дополнительные отметки")]
        public string? CustomInformation { get; set; }
        [InverseProperty("RentalRecord")]        
        public virtual List<RentalItem> RentalItems { get; set; } = new List<RentalItem>();

        [InverseProperty("RentalRecord")]
        public virtual List<PaymentRecord> PaymentsAndRefunds { get; set; } = new List<PaymentRecord>();
        
    }
}
