using ECommerceAPI.Model;

namespace ECommerceAPI.Services
{
    public interface IEmailQueue
    {
        ValueTask QueueEmailAsync(EmailRequest email);

        ValueTask<EmailRequest> DequeueEmailAsync(
            CancellationToken cancellationToken);
    }
}
