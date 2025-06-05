using eTimesheet.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
        var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        mail.To.Add(to);

        await client.SendMailAsync(mail);
    }
}
