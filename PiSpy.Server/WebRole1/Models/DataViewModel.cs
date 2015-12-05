using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WebRole1.Models
{
    public class DataViewModel
    {
        // this logic will present some issues if there is a relatively large span of time where the PiSpy device did not report anything

        public List<PiSpyDataLog> DataLogs { get; set; }
        public PiSpyDevice PiSpyDevice;

        public int Delta { get; set; } // difference in seconds between intervals

        [DisplayName("Start Time")]
        public DateTime? StartTime { get; set; }

        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }

        public DateTime? GetInterval(int intervalNumber)
        {
            return ((DateTime)StartTime).AddSeconds(Delta * intervalNumber);
        }

        public DataViewModel(int piSpyId, DateTime? startTime = null, DateTime? endTime = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            PiSpyDevice = db.PiSpyDevices.Find(piSpyId);

            StartTime = startTime ?? DateTime.Now.AddHours(-29); // db.PiSpyDataLogs.First(m => m.PiSpySerialNumber == PiSpyDevice.SerialNumber).TimeStamp; // passed in value OR the first recorded data point from that PiSpy
            EndTime = endTime ?? DateTime.Now.AddHours(-5); // passed in value OR the current time in central time // subtracting 5 hours for the time zone difference

            var difference = (DateTime)EndTime - (DateTime)StartTime;

            Delta = (int)Math.Round(difference.TotalSeconds / 6);

            this.DataLogs = db.PiSpyDataLogs.Where(m => 
                m.PiSpySerialNumber == PiSpyDevice.SerialNumber 
                && (StartTime == null || m.TimeStamp >= StartTime)
                && (EndTime == null || m.TimeStamp <= EndTime))
                .ToList();
        }
    }
}
