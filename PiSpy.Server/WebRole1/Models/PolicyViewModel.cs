using System;
using System.Linq;
using System.Web.Mvc;

namespace WebRole1.Models
{
    public class PolicyViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public PiSpyDevice PiSpyDevice { get; set; }

        public NumericPolicy NumericPolicy { get; set; }

        public ReminderPolicy ReminderPolicy { get; set; }

        public DisconnectPolicy DisconnectPolicy { get; set; }

        //public EmailPolicyAction EmailPolicyAction { get; set; }

        public string Email { get; set; }
        public string PhoneNumberEmail { get; set; }

        public SelectList NumericPolicyTypeToSelect()
        {
            return new SelectList(db.NumericPolicyTypes, "Id", "Name");
        }

        public SelectList ComparisonOperatorToSelect()
        {
            return new SelectList(db.ComparisonOperators, "Id", "Operator");
        }

        public SelectList PolicyToSelect(int currentPolicyId)
        {
            var currentPolicy = db.Policies.Find(currentPolicyId);
            return new SelectList(db.Policies.Where(m => m.PiSpyDeviceId == currentPolicy.PiSpyDeviceId && m.Id != currentPolicyId).ToList().Where(m => m.Display() != "()").ToList(), "Id", "DisplayMember");
        }

        public PolicyViewModel() { }

        public PolicyViewModel(int piSpyId)
        {
            PiSpyDevice = db.PiSpyDevices.Find(piSpyId);

            //ReminderPolicy reminder = new ReminderPolicy(piSpyId);
            //var user = db.Users.Single(m => m.UserName == HttpContext.Current.User.Identity.Name);
            //EmailPolicyAction = new EmailPolicyAction();
            //EmailPolicyAction.EmailAddress = user.PhoneNumber == null ? "" : user.PhoneNumber + "@" + user.CellularCarrier.EmailExtension;
            //Email = user.Email;
        }
    }
}
