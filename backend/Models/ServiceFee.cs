using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ServiceFee
    {
        public int ServiceFeeID { get; set; }
        public string? ServiceFeeName { get; set; }
        public int ItemCategoryID { get; set; }
        public virtual ItemCategory? ItemCategory { get; set; }
        public double Fee { get; set; } = 1;
    }
}
