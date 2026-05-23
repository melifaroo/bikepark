using Bikepark.Models;
using Bikepark.Models.ViewModels;

namespace Bikepark.Mappers
{
    
    public static class CustomerMappingExtensions
    {
        public static CustomerViewModel ToViewModel( this Customer customer )
        {
            return new CustomerViewModel
            {
                CustomerID = customer.CustomerID,
                CustomerPhoneNumber = customer.CustomerPhoneNumber,
                CustomerFullName = customer.CustomerFullName ,
                CustomerDocumentType = customer.CustomerDocumentType,
                CustomerDocumentSeries = customer.CustomerDocumentSeries,
                CustomerDocumentNumber = customer.CustomerDocumentNumber,
                CustomerEMail = customer.CustomerEMail,
                CustomerInformation = customer.CustomerInformation                
            };            
        }
    }
}