using Business.Utilities.EmailHandler.Models;

namespace Business.Utilities.EmailHandler.Abstract;
public interface IEmailService
{
    void SendMessage(Message message);
}
