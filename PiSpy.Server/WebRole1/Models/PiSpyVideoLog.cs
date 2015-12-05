using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class PiSpyVideoLog
    {
        public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime TimeReceived { get; set; }
        public string IpAddress { get; set; }
        public string FilePath { get; set; }
        public string PiSpySerialNumber { get; set; }
        public PiSpyVideoLog() { }
        public PiSpyVideoLog(byte[] Header, string ip, string BlobFilePath)
        {
            TimeReceived = DateTime.Now;
            IpAddress = ip;
            FilePath = BlobFilePath;

            byte[] timestampbytes = new byte[8];
            System.Array.Copy(Header, 0, timestampbytes, 0, 8);
            double timestamp = System.BitConverter.ToDouble(timestampbytes, 0);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeStamp = epoch.AddSeconds(timestamp);

            byte[] serialnumbytes = new byte[16];
            System.Array.Copy(Header, 8, serialnumbytes, 0, 16);
            PiSpySerialNumber = System.Text.Encoding.UTF8.GetString(serialnumbytes);
        }
    }
}