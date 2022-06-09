using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Number
    {
        [Required]
        [Display(Name = "Номер")]
        public string ItemNumber { get; set; }
    }
}
