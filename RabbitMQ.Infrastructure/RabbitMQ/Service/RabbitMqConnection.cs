using RabbitMQ.Client;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using Microsoft.Extensions.Options;//نصب این لازمه برای option
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ.Service
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqConnection(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            //با استفاده از کلاس RabbitMqOptions که ایجاد کردیم به عنوان واسط که اینجا هارد کد ننویسیم HostName="localhost"
            var factory = new ConnectionFactory
            {
                HostName = _options.Host,
                UserName = _options.UserName,
                Password = _options.Password
            };

            return await factory.CreateConnectionAsync();//اینجا فقط اتصال به Rabbit انجام شد
        }
    }
}
