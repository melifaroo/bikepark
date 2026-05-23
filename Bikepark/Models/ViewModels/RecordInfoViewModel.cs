using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models.ViewModels
{
    public class RecordInfoViewModel
    {
        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Start", ShortName = "From")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? Start { get; set; }

        [Display(Name = "End", ShortName = "To")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? End { get; set; }

        [Display(Name = "Price")]
        public double? Price { get; set; }
    }
}