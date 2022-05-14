using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum RentalStatus
    {
        //       New,        // admin stage - admin has created the initial empty rental order

        [Display(Name = "Запланирован")]
        Scheduled,   // admin stage - admin has composed the preliminary order accordind to customers requirements (items choosed, term determined, customer contact info recorded, fare selected, cost formed),
                     //                  changes are alowed,
                     //                  items status from this rental is changed to reserved (those items could not be added to another rental order)
                     //Paid,       // admin stage - admin has composed the final orded (changes are not alowed) and accept payment from customer,
                     //                  admin informs mechanic the rental order details
        [Display(Name = "Активен")]
        Active,     // mechanic stage - mechanics has issued all rented items to customer,
                    //                  mechanics informs admin that rent is started 
        [Display(Name = "Завершен")]
        Closed,     // mechanic stage - mechanics has reported receiving all (or last) of rented items, has reported all of service to be paid
                    //                  admin has created records of service 
    }
}

