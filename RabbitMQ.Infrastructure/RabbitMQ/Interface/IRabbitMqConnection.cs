using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ.Interface
{
    public interface IRabbitMqConnection//برای ساخت کانکشن است
    {
        Task<IConnection> GetConnectionAsync();
    }
}
