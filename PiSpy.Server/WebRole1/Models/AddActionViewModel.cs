using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class AddActionViewModel
    {
        public string EmailAddress { get; set; }
        public string EmailMessage { get; set; }

        public string HiddenValue { get; set; }

        public virtual Policy Policy { get; set; }

        public AddActionViewModel() { }

        public AddActionViewModel(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Single(m => m.UserName == HttpContext.Current.User.Identity.Name);

            Policy = db.Policies.Find(id);
            EmailAddress = user.PhoneNumber + "@" + user.CellularCarrier.EmailExtension;
            HiddenValue = user.Email;
        }
    }
}
