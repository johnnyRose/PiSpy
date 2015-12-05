using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class MySpiesViewModel
    {
        public List<PiSpyDevice> PiSpyDevices { get; set; }
        public PiSpyDevice PiSpyDevice { get; set; }

        public MySpiesViewModel()
        {
            PiSpyDevices = new ApplicationDbContext().Users.Single(m => m.UserName == HttpContext.Current.User.Identity.Name).PiSpyDevices.ToList();
        }

        public MySpiesViewModel(PiSpyDevice dev)
        {
            PiSpyDevice = dev;
        }
    }
}