using Business.Utilities.EmailHandler.Abstract;
using Business.Utilities.EmailHandler.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Business.Utilities.EmailHandler.Concrete;
public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    public async Task SendMessageAsync(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfiguration.Username, _emailConfiguration.Username));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}