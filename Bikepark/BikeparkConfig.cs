using System.Configuration;

namespace Bikepark
{
    public class BikeparkConfig
    {
        public static readonly string ManagersRole = "BikeparkManagers";
        public static readonly string AdministratorsRole = "BikeparkAdministrators";
        public DateTime WorkingHoursStart { get; set; }
        public  DateTime WorkingHoursEnd{ get; set; }
        public int MinServiceDelayBetweenRentsMinutes { get; set; }
        public int DefaultRentTimeHours { get; set; }
        public int ScheduleWarningTimeMinutes { get; set; }
        public int GetBackWarningTimeMinutes { get; set; }
        public int OnServiceWarningTimeHours { get; set; }

        public int DefaultLogPageSize { get; set; }

        public static void AddOrUpdateAppSetting<T>(string key, T value)
        {
            try
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "bikepark.json");//AppContext.BaseDirectory
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                var sectionPath = key.Split(":")[0];

                if (!string.IsNullOrEmpty(sectionPath))
                {
                    var keyPath = key.Split(":")[1];
                    jsonObj[sectionPath][keyPath] = value;
                }
                else
                {
                    jsonObj[sectionPath] = value; // if no sectionpath just set the value
                }

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
