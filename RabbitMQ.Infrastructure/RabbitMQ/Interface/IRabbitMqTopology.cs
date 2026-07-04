using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ.Interface
{
    public interface IRabbitMqTopology
    {
        Task ConfigureAsync(
        IChannel channel,
        string exchange,
        string queue,
        string routingKey,
        string exchangeType = ExchangeType.Direct);
    }
}
