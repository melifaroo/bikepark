
namespace Bikepark.Models
{
    public class DayOfWeekLiterals
    {

        public static readonly DayOfWeekLiterals Monday = new(DayOfWeek.Monday, "Mo");
        public static readonly DayOfWeekLiterals Tuesday = new(DayOfWeek.Tuesday, "Tu");
        public static readonly DayOfWeekLiterals Wednesday = new(DayOfWeek.Wednesday, "We");
        public static readonly DayOfWeekLiterals Thursday = new(DayOfWeek.Thursday, "Th");
        public static readonly DayOfWeekLiterals Friday = new(DayOfWeek.Friday, "Fr");
        public static readonly DayOfWeekLiterals Saturday = new(DayOfWeek.Saturday, "Sa");
        public static readonly DayOfWeekLiterals Sunday = new(DayOfWeek.Sunday, "Su");

        public static readonly Dictionary<DayOfWeek, DayOfWeekLiterals> Map = new Dictionary<DayOfWeek, DayOfWeekLiterals> {
            { DayOfWeek.Monday, Monday },
            { DayOfWeek.Tuesday, Tuesday },
            { DayOfWeek.Wednesday, Wednesday },
            { DayOfWeek.Thursday, Thursday },
            { DayOfWeek.Friday, Friday },
            { DayOfWeek.Saturday, Saturday },
            { DayOfWeek.Sunday, Sunday }
        };

        public DayOfWeek DayOfWeek { get; }
        public string ShortRuName { get; }
        private DayOfWeekLiterals(DayOfWeek dayOfWeek, string shortRuName)
        {
            DayOfWeek = dayOfWeek;
            ShortRuName = shortRuName;
        }

        public static DayOfWeekLiterals Today() {
            return Map.GetValueOrDefault(DateTime.Today.DayOfWeek)!;
        }

        public static DayOfWeekLiterals ForDay(DayOfWeek dayOfWeek) {
            return Map.GetValueOrDefault(dayOfWeek)!;
        }

        public override string ToString()
        {
            return ShortRuName;
        }
    }
}