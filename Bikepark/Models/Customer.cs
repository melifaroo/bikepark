using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Customer
    {
        public int? CustomerID { get; set; }
        [Display(Name = "Контактный телефон")]
        public string? CustomerContactNumber { get; set; }
        [Display(Name = "Имя/Фамилия")]
        public string? CustomerFullName { get; set; }
        [Display(Name = "Документ")]
        public string? CustomerPassport { get; set; }
        [Display(Name = "e-Mail")]
        public string? CustomerEMail { get; set; }
    }
}
