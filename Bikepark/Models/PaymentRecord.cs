using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class PaymentRecord
    {
        public int PaymentRecordID { get; set; }
        public DateTime? DateTime { get; set; }
        public double? Value { get; set; }
        public bool Executed  { get; set; }
        public int? RentalRecordID  { get; set; }
        public virtual RentalRecord? RentalRecord { get; set; }       
        public int? ServiceRecordID { get; set; }
        public virtual ServiceRecord? ServiceRecord { get; set; }
    }
}
