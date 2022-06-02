using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum Status
    {
        [Display(Name = "Не назначен")]
        Draft,      
        [Display(Name = "Запланирован")]
        Scheduled,   
        [Display(Name = "Выдан")]
        Active,     
        [Display(Name = "Завершен")]
        Closed,

        [Display(Name = "Подлежит ремонту")]
        Service,
        [Display(Name = "Запланирован ремонт")]
        ServiceScheduled,
        [Display(Name = "В ремонте")]
        OnService,
        [Display(Name = "Отремонтирован")]
        Fixed,     
    }
}

