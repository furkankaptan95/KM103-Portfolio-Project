using Ardalis.Result;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace App.Services;
public class SmtpConfiguration
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}
public class SmtpEmailService : IEmailService
{
    private readonly SmtpConfiguration _smtpConfiguration;
    public SmtpEmailService(IOptions<SmtpConfiguration> smtpConfiguration)
    {
        _smtpConfiguration = smtpConfiguration.Value;
    }

    public async Task<Result> SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpConfiguration.Username),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            message.To.Add(to);

            var smtpClient = new SmtpClient(_smtpConfiguration.Server)
            {
                Port = _smtpConfiguration.Port,
                Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password),
                EnableSsl = _smtpConfiguration.EnableSsl
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