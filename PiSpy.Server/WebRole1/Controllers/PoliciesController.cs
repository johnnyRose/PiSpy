using System.Web.Mvc;
using WebRole1.Models;
using System.Linq;

namespace WebRole1.Controllers
{
    [Authorize]
    public class PoliciesController : Controller
    {
        public ActionResult Index(int id)
        {
            return View(new PolicyViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id, PolicyViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //model.NumericPolicy.PolicyAction = model.EmailPolicyAction;
            db.PiSpyDevices.Find(id).Policies.Add(model.NumericPolicy);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGroup(int id, string name)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var policy = PolicyFactory.GetPolicyGroup(name);
            db.PiSpyDevices.Find(id).Policies.Add(policy);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToGroup(int groupId, int policyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var policy = db.Policies.Find(policyId);
            var policyGroup = db.PolicyGroups.Find(groupId);
            policy.PiSpyDeviceId = null;
            db.SaveChanges();
            policyGroup.Policies.Add(policy);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = policyGroup.PiSpyDeviceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePolicy(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var policy = db.Policies.Find(id);
            int redirectId = (int)policy.PiSpyDeviceId;
            policy.PiSpyDeviceId = null;
            //DeleteAllPolicies(id);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = redirectId });
        }

        public ActionResult AddAction(int id)
        {
            return View(new AddActionViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAction(int id, AddActionViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            EmailPolicyAction action = new EmailPolicyAction()
            {
                EmailAddress = model.EmailAddress,
                EmailMessage = model.EmailMessage,
            };

            Policy policy = db.Policies.Find(id);
            policy.PolicyAction = action;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = policy.PiSpyDeviceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReminder(int id, PolicyViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.PiSpyDevices.Find(id).Policies.Add(model.ReminderPolicy);
            db.SaveChanges();

            return RedirectToAction("AddAction", new { id = model.ReminderPolicy.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDisconnect(int id, PolicyViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.PiSpyDevices.Find(id).Policies.Add(model.DisconnectPolicy);
            db.SaveChanges();

            return RedirectToAction("AddAction", new { id = model.DisconnectPolicy.Id });
        }

        //private void DeleteAllPolicies(int id)
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    var policy = db.Policies.Find(id);

        //    if (policy.GetType().Name.Contains("Group"))
        //    {
        //        foreach (var pol in policy.PolicyGroups)
        //        {
        //            if (pol.PolicyGroups.Count == 0)
        //            {
        //                db.Policies.Remove(db.Policies.Find(pol.Id));
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                DeleteAllPolicies(pol.Id);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var pol in policy.PolicyGroups)
        //        {
        //            if (pol.Policies.Count == 0)
        //            {
        //                db.Policies.Remove(db.Policies.Find(pol.Id));
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                DeleteAllPolicies(pol.Id);
        //            }
        //        }
        //    }

        //    db.SaveChanges();
        //}
    }
}
