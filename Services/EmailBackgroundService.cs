namespace ECommerceAPI.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly IEmailSender _emailSender;
        //private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundService> _logger;

        public EmailBackgroundService(
        IEmailQueue emailQueue,
        IEmailSender emailSender,
        ILogger<EmailBackgroundService> logger)
        {
            _emailQueue = emailQueue;
            _emailSender = emailSender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var email = await _emailQueue
                        .DequeueEmailAsync(stoppingToken);

                    await _emailSender.SendEmailAsync(email.To, email.Subject, email.Body)
                         .ContinueWith(task =>
                         {
                             if (task.IsFaulted)
                             {
                                 _logger.LogError(task.Exception, "Failed to send email to {Email}", email.To);
                             }
                             else
                             {
                                 _logger.LogInformation("Email sent to {Email}", email.To);
                             }
                         }, stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error processing email queue");
                }

            }
        }
    }
}
