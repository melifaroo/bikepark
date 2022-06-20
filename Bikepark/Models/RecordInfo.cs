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

        public int AttentionStatus  { get; set; }= 0;

        public void Attention(int getBackWarningTimeMinutes, int scheduleWarningTimeMinutes, int onServiceWarningTimeHours)
        {
            AttentionStatus = (Status == Status.Active && End < DateTime.Now) ? 0 :
                    (Status == Status.Scheduled && Start < DateTime.Now) ? 1 :
                    (Status == Status.Service) ? 2 :
                    (Status == Status.OnService && End < DateTime.Now) ? 3 :
                    (Status == Status.Active && End < DateTime.Now.AddMinutes(getBackWarningTimeMinutes)) ? 4 :
                    (Status == Status.Scheduled && Start < DateTime.Now.AddMinutes(scheduleWarningTimeMinutes)) ? 5 :
                    (Status == Status.OnService && Start < DateTime.Now.AddHours(onServiceWarningTimeHours)) ? 6 :
                    (Status == Status.Draft) ? 7 :
                    (Status == Status.Active) ? 8 :
                    (Status == Status.Scheduled) ? 9 :
                    (Status == Status.OnService) ? 10 : 11;
        }
    }
}
