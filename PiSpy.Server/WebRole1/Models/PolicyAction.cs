using System;

namespace WebRole1.Models
{
    public class PolicyAction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime? LastExecuted { get; set; }

        public virtual void Execute(object piSpyDataLog) { }
    }

    public class EmailPolicyAction : PolicyAction
    {
        public string EmailAddress { get; set; }
        public string EmailMessage { get; set; }

        public override void Execute(object piSpyDataLog)
        {
            var log = (PiSpyDataLog)piSpyDataLog;
            string newMessage = EmailMessage.Replace("{temperature}", log.TemperatureFahrenheit.ToString()).Replace("{humidity}", log.Humidity.ToString());
            new EmailInfo(EmailAddress, newMessage).Send();
        }

        public override string ToString()
        {
            return "email " + EmailAddress + " the message \"" + EmailMessage + "\"";
        }
    }
}
