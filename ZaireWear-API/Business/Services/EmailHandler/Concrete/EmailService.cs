using Business.Services.EmailHandler.Models;
using Business.Services.EmailHandler.Abstract;
using MailKit.Net.Smtp;
using MimeKit;

namespace Business.Services.EmailHandler.Concrete;
public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage, cancellationToken);
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

    private async Task SendAsync(MimeMessage message, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true, cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}