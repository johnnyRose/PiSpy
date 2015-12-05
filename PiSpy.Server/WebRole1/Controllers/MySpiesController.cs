using System.Web.Mvc;
using WebRole1.Models;
using System.Linq;

namespace WebRole1.Controllers
{
    [Authorize]
    public class MySpiesController : Controller
    {
        public ActionResult Index()
        {
            return View(new MySpiesViewModel());
        }

        public ActionResult Add()
        {
            return View(new PiSpyDevice());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PiSpyDevice model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string appUserId = db.Users.Single(m => m.UserName == User.Identity.Name).Id;
                model.ApplicationUserId = appUserId;
                db.PiSpyDevices.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View("Add", new ApplicationDbContext().PiSpyDevices.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PiSpyDevice model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var piSpy = db.PiSpyDevices.Find(model.Id);

                piSpy.Name = model.Name;
                piSpy.SerialNumber = model.SerialNumber;
                piSpy.Location = model.Location;
                piSpy.Notes = model.Notes;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Add", model);
        }

        public ActionResult Delete(int id)
        {
            return View(new ApplicationDbContext().PiSpyDevices.Find(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.PiSpyDevices.Remove(db.PiSpyDevices.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
