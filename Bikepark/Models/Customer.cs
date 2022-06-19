using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Customer
    {
        public int? CustomerID { get; set; }
        [Display(Name = "Контактный телефон")]
        public string? CustomerPhoneNumber { get; set; }
        [Display(Name = "Имя Фамилия")]
        public string? CustomerFullName { get; set; }
        [Display(Name = "Документ")]
        public string? CustomerDocumentType { get; set; }
        [Display(Name = "Серия документа")]
        public string? CustomerDocumentSeries { get; set; }
        [Display(Name = "Номер документа")]
        public string? CustomerDocumentNumber { get; set; }
        [Display(Name = "e-Mail")]
        public string? CustomerEMail { get; set; }
        [Display(Name = "Дополнительно")]
        public string? CustomerInformation { get; set; }
    }
}
