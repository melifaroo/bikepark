
namespace Bikepark.Models
{
    public static class DayOfWeekLiterals
    {

        public static string GetDaysString(DaysOfWeekFlags days)
        {
            string[] names = { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
            var selected = new List<string>();
            int value = (int)days;

            for (int i = 0; i < 7; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    selected.Add(names[i]);
                }
            }
            return string.Join(", ", selected);
        }

        // public static readonly DayOfWeekLiterals Monday = new(DayOfWeek.Monday, "Mo");
        // public static readonly DayOfWeekLiterals Tuesday = new(DayOfWeek.Tuesday, "Tu");
        // public static readonly DayOfWeekLiterals Wednesday = new(DayOfWeek.Wednesday, "We");
        // public static readonly DayOfWeekLiterals Thursday = new(DayOfWeek.Thursday, "Th");
        // public static readonly DayOfWeekLiterals Friday = new(DayOfWeek.Friday, "Fr");
        // public static readonly DayOfWeekLiterals Saturday = new(DayOfWeek.Saturday, "Sa");
        // public static readonly DayOfWeekLiterals Sunday = new(DayOfWeek.Sunday, "Su");

        // public static readonly Dictionary<DayOfWeek, DayOfWeekLiterals> Map = new Dictionary<DayOfWeek, DayOfWeekLiterals> {
        //     { DayOfWeek.Monday, Monday },
        //     { DayOfWeek.Tuesday, Tuesday },
        //     { DayOfWeek.Wednesday, Wednesday },
        //     { DayOfWeek.Thursday, Thursday },
        //     { DayOfWeek.Friday, Friday },
        //     { DayOfWeek.Saturday, Saturday },
        //     { DayOfWeek.Sunday, Sunday }
        // };

        // public DayOfWeek DayOfWeek { get; }
        // public string ShortRuName { get; }
        // private DayOfWeekLiterals(DayOfWeek dayOfWeek, string shortRuName)
        // {
        //     DayOfWeek = dayOfWeek;
        //     ShortRuName = shortRuName;
        // }

        // public static DayOfWeekLiterals Today() {
        //     return Map.GetValueOrDefault(DateTime.Today.DayOfWeek)!;
        // }

        // public static DayOfWeekLiterals ForDay(DayOfWeek dayOfWeek) {
        //     return Map.GetValueOrDefault(dayOfWeek)!;
        // }

        // public override string ToString()
        // {
        //     return ShortRuName;
        // }
    }
}