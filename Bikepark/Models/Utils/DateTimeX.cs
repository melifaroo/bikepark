namespace Bikepark.Models
{
    public class DateTimeX
    {
        public static DateTime Max(DateTime date1, DateTime date2)
        {
            return (date1 > date2 ? date1 : date2);
        }
    }
}
