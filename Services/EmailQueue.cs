using ECommerceAPI.Model;
using System.Threading.Channels;

namespace ECommerceAPI.Services
{
    public class EmailQueue :IEmailQueue
    {
        private readonly Channel<EmailRequest> _queue;

        public EmailQueue()
        {
            _queue = Channel.CreateUnbounded<EmailRequest>();
        }

        public async ValueTask QueueEmailAsync(EmailRequest email)
        {
            await _queue.Writer.WriteAsync(email);
        }

        public async ValueTask<EmailRequest> DequeueEmailAsync(
            CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
