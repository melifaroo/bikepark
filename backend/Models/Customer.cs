namespace backend.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        public string? FullName { get; set; }
        public string? PassportID { get; set; }
        public string? ContactNumber { get; set; }
        public string? EMail { get; set; }

    }
}
