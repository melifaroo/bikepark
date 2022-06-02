using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class RecordInfo
    {
        [Display(Name = "Статус")]
        public Status Status { get; set; } = Status.Draft;

        [Display(Name = "Начало", ShortName = "C")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? Start { get; set; }

        [Display(Name = "Завершение", ShortName = "До")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? End { get; set; }

        [Display(Name = "Дополнительные отметки")]
        public string? CustomInformation { get; set; }

        public string? UserID { get; set; }
        [Display(Name = "Администратор")]
        public virtual IdentityUser? User { get; set; }

        public static bool Overlap(RecordInfo a, RecordInfo b)
        {
            return  a.Status != Models.Status.Closed && 
                    a.Status != Models.Status.Fixed  && 
                    b.Status != Models.Status.Closed &&  
                    b.Status != Models.Status.Fixed  && 
                    (a.Start <= ( (b.Status == Models.Status.Active || b.Status == Models.Status.OnService) ? DateTimeX.Max(b.End ?? DateTime.Now, DateTime.Now).AddHours(1) : b.End ) ) &&
                    (b.Start <= ( (a.Status == Models.Status.Active || a.Status == Models.Status.OnService) ? DateTimeX.Max(a.End ?? DateTime.Now, DateTime.Now).AddHours(1) : a.End ) ) ;
        }

    }
}
