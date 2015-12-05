using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace WebRole1.Models
{
    public class PiSpyDevice
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }

        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual List<Policy> Policies { get; set; }

        // public string Secret { get; set; } // we need a secret string to prevent people from registering other users' pispies

        public void CheckPolicies(PiSpyDataLog log)
        {
            foreach (var policy in Policies.Where(m => m.IsSatisfied(log) && m.PolicyActionId != null))
            {
                //policy.PolicyAction.Execute(); // can't put this here. has to go through website

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://pispy.cloudapp.net/Policies/TriggerPolicy/" + policy.Id); // PoliciesController, TriggerPolicy action
                request.Timeout = 20000; // 20 seconds
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();
            }
        }
    }
}
