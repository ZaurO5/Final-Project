﻿using MimeKit;

namespace Business.Services.EmailHandler.Models;

public class Message
{
    public string Subject { get; set; }
    public string Content { get; set; }
    public List<MailboxAddress> To { get; set; }

    public Message(List<string> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(email => new MailboxAddress(email, email)));
        Subject = subject;
        Content = content;
    }
}