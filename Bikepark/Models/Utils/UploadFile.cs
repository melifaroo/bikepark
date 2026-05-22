using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class UploadFile : ReponseModel
    {
        [Required(ErrorMessage = "Choose Form File ")]
        public IFormFile? File { get; set; }
    }
}
