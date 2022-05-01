namespace Bikepark.Models
{
    public enum ServiceStatus
    {
       // New,        // admin stage - admin has created the initial empty rental order
        Assigned,   // admin stage - admin has assigned the preliminary order accordind to customers requirements (items choosed, term determined, customer contact info recorded, fare selected, cost formed),
                    //                  changes are alowed,
                    //                  items status from this rental is changed to reserved (those items could not be added to another rental order)
       // Paid,       // admin stage - admin has accepted payment from customer,
                    //                  admin informs mechanic the rental order details
        InProcess,     // mechanic stage - mechanics has issued all rented items to customer,
                    //                  mechanics informs admin that rent is started 
        Completed,     // mechanic stage - mechanics has reported receiving all (or last) of rented items, has reported all of service to be paid
                    //                  asmin creates 
    }
}

