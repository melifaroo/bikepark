namespace Bikepark.Models
{
    public class ReponseModel
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool HasWarnings { get; set; }
        public bool IsResponse { get; set; }
    }
}
