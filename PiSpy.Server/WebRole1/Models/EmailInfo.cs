using SendGrid;
using System.Net;
using System.Net.Mail;

namespace WebRole1.Models
{
    public class EmailInfo
    {
        public string ToAddress { get; set; }
        public string Body { get; set; }

        public EmailInfo(string to, string body)
        {
            ToAddress = to;
            Body = body;
        }

        public void Send()
        {
            var myMessage = new SendGridMessage();
            //myMessage.From = new MailAddress("pispy@pispy.cloudapp.net");
            //myMessage.From = new MailAddress("skyspi@skyspi.cloudapp.net");
            myMessage.From = new MailAddress("klayton@klayton.cloudapp.net");
                
            myMessage.AddTo(this.ToAddress);

            myMessage.Subject = "PiSpy Notification";
            myMessage.Text = this.Body;

            //John
            //var credentials = new NetworkCredential(
            //    "azure_0de8c1d717c8ec8c1a6cacd742b5866c@azure.com",
            //    "69g8nntNJB1zm59");
            
            //Jeff
            //var credentials = new NetworkCredential(
            //    "azure_27448683e5c088dc2ebbf7448cbe975a@azure.com",
            //    "Sn6Rz2gVDeZlbG1");
            
            //Klayton 
            var credentials = new NetworkCredential(
                "azure_38976e066d5cac4cd047a666e5d15a81@azure.com",
                "8IL1tfpWYGquj6a");

            var transportWeb = new Web(credentials);

            var results = transportWeb.DeliverAsync(myMessage);
        }
    }
}
