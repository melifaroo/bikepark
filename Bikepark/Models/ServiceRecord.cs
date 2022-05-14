using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ServiceRecord
    {
        public int ServiceRecordID { get; set; }
        public ServiceStatus ServiceStatus { get; set; }        
        public int? ItemID { get; set; }
        public virtual Item? Item { get; set; }
        public virtual List<ServiceFee> ServiceFees { get; set; } = new List<ServiceFee>();

        [InverseProperty("ServiceRecord")]
        public virtual List<PaymentRecord> Payments { get; set; } = new List<PaymentRecord>();
        public DateTime? ServiceStartTime { get; set; }
        public DateTime? ServiceEndTime { get; set; }
        public int? RentalRecordID { get; set; }
        public virtual RentalRecord? RentalRecord { get; set; }
        public bool Maintenance { get; set; }
    }
}
