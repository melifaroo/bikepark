using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Number : ReponseModel
    {
        [Display(Name = "Number")]
        [Required(ErrorMessage = "Provice Item Number")]
        public string? ItemNumber { get; set; }
    }
}
