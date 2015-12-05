using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class StreamViewModel
    {
        public PiSpyDevice PiSpyDevice { get; set; }

        public string SourceOfLastVideo { get; set; }

        public StreamViewModel(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.PiSpyDevice = db.PiSpyDevices.Find(id);
            this.SourceOfLastVideo = db.PiSpyVideoLogs.Where(m => m.PiSpySerialNumber == this.PiSpyDevice.SerialNumber).ToList().Last().FilePath;
        }
    }
}
