using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace WebRole1.Models
{
    public static class PolicyEngine
    {
        public static void CheckPolicies()
        {
            while (true)
            {
                // hit website to keep it from falling asleep
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://pispy.cloudapp.net");
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://skyspi.cloudapp.net");
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://klayton.cloudapp.net");
                request.Timeout = 20000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                ApplicationDbContext db = new ApplicationDbContext();

                var pispies = db.PiSpyDevices.ToList();

                foreach (var pispy in pispies)
                {
                    PiSpyDataLog lastLog = db.PiSpyDataLogs.Where(m => m.PiSpySerialNumber == pispy.SerialNumber).OrderByDescending(m => m.Id).FirstOrDefault();

                    if (lastLog != null && !lastLog.TriggeredNumericPolicy) // if it exists and it hasn't already been triggered
                    {
                        foreach (var policy in pispy.Policies)
                        {
                            // if the policy is satisfied and the last time the policy has been executed was greater than or equal to the time the user specifies
                            if (policy.IsSatisfied(lastLog) && policy.PolicyActionId != null &&
                                (policy.PolicyAction.LastExecuted == null || ((DateTime)policy.PolicyAction.LastExecuted).AddMinutes(policy.PiSpyDevice.ApplicationUser.MinutesBetweenAlerts) <= DateTime.Now))
                            {
                                policy.PolicyAction.Execute(lastLog);
                                policy.PolicyAction.LastExecuted = DateTime.Now;
                                lastLog.TriggeredNumericPolicy = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }

                Thread.Sleep(15000); // 15 seconds
            }
        }
    }
}
