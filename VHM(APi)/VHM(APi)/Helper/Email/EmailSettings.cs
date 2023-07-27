using System.Net;
using System.Net.Mail;

namespace VHM_APi_.Helper.Email
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.sendgrid.net", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("", "");
            client.Send("email.com", email.To, email.Subject, email.Body);
        }
    }
}
