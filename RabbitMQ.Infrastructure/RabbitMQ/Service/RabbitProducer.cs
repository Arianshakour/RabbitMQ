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

        private readonly IRabbitMqTopology _topology;

        public RabbitProducer(IRabbitMqConnection rabbitConnection, IRabbitMqTopology topology)
        {
            _rabbitConnection = rabbitConnection;
            _topology = topology;
        }

        public async Task PublishAsync<T>(string exchange, string queue, string routingKey,T message)
        {
            var connection = await _rabbitConnection.GetConnectionAsync();//اول کانکشن را میگیریم

            //ساخت چنل
            await using var channel =
                await connection.CreateChannelAsync();

            //برای تمیزی بردیم در یک کلاس کمکی
            await _topology.ConfigureAsync(channel,exchange,queue,routingKey);

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
