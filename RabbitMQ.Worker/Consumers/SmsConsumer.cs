using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using RabbitMQ.Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQ.Worker.Consumers
{
    public class SmsConsumer 
    {
        private readonly IRabbitMqConnection _connection;

        public SmsConsumer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            //ساخت کانکشن
            var connection =
                await _connection.GetConnectionAsync();

            //ساخت چنل
            var channel =
                await connection.CreateChannelAsync();


            //ساخت Exchange
            await channel.ExchangeDeclareAsync(
                exchange: "notification-exchange",
                type: ExchangeType.Direct,
                durable: true);


            //ساخت Queue
            await channel.QueueDeclareAsync(
                queue: "sms-queue",
                durable: true,
                exclusive: false,
                autoDelete: false);

            //Bind
            await channel.QueueBindAsync(
                queue: "sms-queue",
                exchange: "notification-exchange",
                routingKey: "sms");


            Console.WriteLine("Sms Consumer Started...");

            //یک لیسنر بساز که بتواند پیام‌های این چنل را دریافت کند
            var consumer = new AsyncEventingBasicConsumer(channel);

            //هر وقت ربیت یک پیام برای این کانسومر فرستاد میره توی متد پایین
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer.Deserialize<NotificationMessage>(json);

                Console.WriteLine("----------------------------------");
                Console.WriteLine("Sending Sms...");
                Console.WriteLine($"Name   : {message?.FullName}");
                Console.WriteLine($"Mobile : {message?.Mobile}");

                await Task.Delay(2000);

                Console.WriteLine("Sms Sent Successfully");
                Console.WriteLine("----------------------------------");
            };

            await channel.BasicConsumeAsync(
                queue: "sms-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("Waiting For Messages...");

            //اگه اینو ننویسی برنامه بسته میشه درحالیکه Worker همیشه باید باز و منتظر باشه
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
