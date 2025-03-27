using Business.Services.EmailHandler.Models;

namespace Business.Services.EmailHandler.Abstract;
public interface IEmailService
{
    Task SendMessageAsync(Message message, CancellationToken cancellationToken);
}
