using System.Net;
using System.Net.Mail;

namespace CarRent_CarRentalWebApp.Helpers;
public static class MailExtension
{
    public static void SendMessage(string email, string subject, string body) 
    {
        MailMessage mail=new MailMessage();
        mail.From=new MailAddress("carrentaffordablecars@gmail.com");
        mail.To.Add(new MailAddress(email));
        mail.Subject=subject;   
        mail.Body=body;

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Port = 587;
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.EnableSsl=true;
        smtpClient.Credentials = new NetworkCredential("carrentaffordablecars@gmail.com", "gbncxrkvmldwffpn");
        smtpClient.Send(mail);


    }
}
