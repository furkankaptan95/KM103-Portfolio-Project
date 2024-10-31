using App.Services;
using Ardalis.Result;
using System.Net.Mail;
using System.Net;

namespace App.DataAPI.Services;
public class SmtpEmailService : IEmailService
{
    public async Task<Result> SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress("Furkan.Kaptan.Work@gmail.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            message.To.Add(to);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("Furkan.Kaptan.Work@gmail.com", "tsfk lmxj qhlz kuju"),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Error();
        }
    }
}
