using System.Web.Mvc;
using WebRole1.Models;
using System.Linq;

namespace WebRole1.Controllers
{
    public class StreamingController : Controller
    {
        public ActionResult Stream(int id)
        {
            return View(new StreamViewModel(id));
        }

        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetLatest(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            PiSpyDevice device = db.PiSpyDevices.Find(id);
            string path = db.PiSpyVideoLogs.Where(m => m.PiSpySerialNumber == device.SerialNumber).ToList().Last().FilePath;

            return Json(path, JsonRequestBehavior.AllowGet);
        }
    }
}
