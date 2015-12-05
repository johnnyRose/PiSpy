using System;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        public ActionResult Climate(int id)
        {
            return View(new DataViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Climate(int id, DateTime? startTime, DateTime? endTime)
        {
            return View(new DataViewModel(id, startTime, endTime));
        }
    }
}
