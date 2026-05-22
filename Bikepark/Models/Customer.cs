using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Customer
    {
        public int? CustomerID { get; set; }

        [Display(Name = "Phone Number")]
        public string? CustomerPhoneNumber { get; set; }

        [Display(Name = "Full Name")]
        public string? CustomerFullName { get; set; }

        [Display(Name = "Identity Document")]
        public string? CustomerDocumentType { get; set; }

        [Display(Name = "Document Serie")]
        public string? CustomerDocumentSeries { get; set; }

        [Display(Name = "Document Number")]
        public string? CustomerDocumentNumber { get; set; }

        [Display(Name = "E-Mail")]
        public string? CustomerEMail { get; set; }

        [Display(Name = "Additional Information")]
        public string? CustomerInformation { get; set; }
    }
}
