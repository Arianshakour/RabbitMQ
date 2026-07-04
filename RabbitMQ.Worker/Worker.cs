using RabbitMQ.Worker.Consumers;

namespace RabbitMQ.Worker
{
    public class Worker : BackgroundService
    {
        private readonly SmsConsumer _smsConsumer;
        private readonly EmailConsumer _emailConsumer;

        public Worker(
            SmsConsumer smsConsumer,
            EmailConsumer emailConsumer)
        {
            _smsConsumer = smsConsumer;
            _emailConsumer = emailConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Worker Started...");

            // هر دو Consumer همزمان اجرا شوند
            await Task.WhenAll(
                _smsConsumer.ConsumeAsync(stoppingToken),
                _emailConsumer.ConsumeAsync(stoppingToken));
        }
    }
}
