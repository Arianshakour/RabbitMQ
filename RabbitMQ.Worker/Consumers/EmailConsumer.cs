using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Worker.Models;
using System.Text.Json;

namespace RabbitMQ.Worker.Consumers
{
    public class EmailConsumer 
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IRabbitMqTopology _topology;


        public EmailConsumer(IRabbitMqConnection connection, IRabbitMqTopology topology)
        {
            _connection = connection;
            _topology = topology;
        }

        public async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            //ساخت کانکشن
            var connection =
                await _connection.GetConnectionAsync();

            //ساخت چنل
            var channel =
                await connection.CreateChannelAsync();


            //برای تمیزی بردیم در یک کلاس کمکی
            await _topology.ConfigureAsync(
                channel,
                exchange: "notification-exchange",
                queue: "sms-queue",
                routingKey: "sms");


            Console.WriteLine("Email Consumer Started...");

            //یک لیسنر بساز که بتواند پیام‌های این چنل را دریافت کند
            var consumer = new AsyncEventingBasicConsumer(channel);

            //هر وقت ربیت یک پیام برای این کانسومر فرستاد میره توی متد پایین
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer.Deserialize<NotificationMessage>(json);

                Console.WriteLine("----------------------------------");
                Console.WriteLine("Sending Email...");
                Console.WriteLine($"Name   : {message?.FullName}");
                Console.WriteLine($"Email : {message?.Email}");

                await Task.Delay(2000);

                Console.WriteLine("Email Sent Successfully");
                Console.WriteLine("----------------------------------");
            };

            await channel.BasicConsumeAsync(
                queue: "email-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("Waiting For Emails...");

            //اگه اینو ننویسی برنامه بسته میشه درحالیکه Worker همیشه باید باز و منتظر باشه
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
