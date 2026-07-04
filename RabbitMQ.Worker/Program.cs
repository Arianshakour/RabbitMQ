using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using RabbitMQ.Infrastructure.RabbitMQ.Service;
using RabbitMQ.Infrastructure.RabbitMQ;
using RabbitMQ.Worker;
using RabbitMQ.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IRabbitMqTopology, RabbitMqTopology>();

builder.Services.AddSingleton<SmsConsumer>();

builder.Services.AddSingleton<EmailConsumer>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
