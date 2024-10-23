using Ardalis.Result;

namespace App.Services;
public interface IEmailService
{
    Task<Result> SendEmailAsync(string to, string subject, string htmlMessage);
}
