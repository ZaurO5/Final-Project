namespace Business.Services.Abstract;

public interface IPaymentService
{
    Task<(int statusCode, string? description, string? url)> PayAsync();
    Task<bool> PaySuccess(Guid token);
    Task<bool> PayCancel(Guid token);
}
