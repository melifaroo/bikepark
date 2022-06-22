using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Number : ReponseModel
    {
        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Укажите номер велосипеда")]
        public string ItemNumber { get; set; }
    }
}
