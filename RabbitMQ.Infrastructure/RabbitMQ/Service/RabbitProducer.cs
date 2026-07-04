using RabbitMQ.Application.Interface;
using RabbitMQ.Client;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ.Service
{
    public class RabbitProducer : IRabbitProducer
    {
        private readonly IRabbitMqConnection _rabbitConnection;
        // برای این اینجکت کردم که Producer نباید بلد باشد Connection بسازد
        // فقط میگه اتصال بده مثل DBContext

        public RabbitProducer(IRabbitMqConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public async Task PublishAsync<T>(string exchange, string queue, string routingKey,T message)
        {
            var connection = await _rabbitConnection.GetConnectionAsync();//اول کانکشن را میگیریم

            //ساخت چنل
            await using var channel =
                await connection.CreateChannelAsync();

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

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            //پابلیش
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

        }
    }
}
