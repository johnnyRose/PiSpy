using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRole1.Models
{
    public class PiSpyDataLog
    {
        public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string IpAddress { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string PiSpySerialNumber { get; set; }
        public DateTime TimeReceived { get; set; }

        public bool TriggeredNumericPolicy { get; set; }

        [NotMapped]
        public double TemperatureFahrenheit
        {
            get
            {
                return (Temperature * (9.0 / 5.0)) + 32;
            }
        }

        [NotMapped]
        public double DewPoint
        {
            get
            {
                return TemperatureFahrenheit - ((100 - Humidity) / 5);
            }
        }

        public PiSpyDataLog() { }

        public PiSpyDataLog(string dataString, string ip)
        {
            TimeReceived = DateTime.Now;
            IpAddress = ip;

            var args = dataString.Split(';');

            PiSpySerialNumber = args[0];
            Temperature = double.Parse(args[1]);
            Humidity = double.Parse(args[2]);
            TimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(int.Parse(args[3])).AddHours(-5); // timestamp from the pi, subtracting 5 hours to adjust for the time zone difference 
        }
    }
}
