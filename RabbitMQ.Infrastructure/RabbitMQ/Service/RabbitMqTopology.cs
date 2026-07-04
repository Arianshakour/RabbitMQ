using RabbitMQ.Application.Interface;
using RabbitMQ.Client;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ.Service
{
    public class RabbitMqTopology : IRabbitMqTopology
    {
        public async Task ConfigureAsync(IChannel channel, string exchange, string queue, string routingKey, string exchangeType = "direct")
        {
            //ساخت Exchange
            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true);

            // ساخت Queue
            await channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind
            await channel.QueueBindAsync(
                queue: queue,
                exchange: exchange,
                routingKey: routingKey);
        }
    }
}
