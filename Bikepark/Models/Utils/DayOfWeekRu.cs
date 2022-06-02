
namespace Bikepark.Models
{
    public class DayOfWeekRu
    {

        public static readonly DayOfWeekRu Monday = new DayOfWeekRu(DayOfWeek.Monday, "Пн");
        public static readonly DayOfWeekRu Tuesday = new DayOfWeekRu(DayOfWeek.Tuesday, "Вт");
        public static readonly DayOfWeekRu Wednesday = new DayOfWeekRu(DayOfWeek.Wednesday, "Ср");
        public static readonly DayOfWeekRu Thursday = new DayOfWeekRu(DayOfWeek.Thursday, "Чт");
        public static readonly DayOfWeekRu Friday = new DayOfWeekRu(DayOfWeek.Friday, "Пт");
        public static readonly DayOfWeekRu Saturday = new DayOfWeekRu(DayOfWeek.Saturday, "Сб");
        public static readonly DayOfWeekRu Sunday = new DayOfWeekRu(DayOfWeek.Sunday, "Вс");

        public static readonly Dictionary<DayOfWeek, DayOfWeekRu> Map = new Dictionary<DayOfWeek, DayOfWeekRu> {
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
        private DayOfWeekRu(DayOfWeek dayOfWeek, string shortRuName)
        {
            DayOfWeek = dayOfWeek;
            ShortRuName = shortRuName;
        }

        public static DayOfWeekRu Today() {
            return Map.GetValueOrDefault(DateTime.Today.DayOfWeek);
        }

        public static DayOfWeekRu ForDay(DayOfWeek dayOfWeek) {
            return Map.GetValueOrDefault(dayOfWeek);
        }

        public override string ToString()
        {
            return ShortRuName;
        }
    }
}