using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.RabbitMQ
{
    public class RabbitMqOptions//این کلاس برای تمیز تر شدن کد و استفاده نکردن HostName="localhost" وسط کد است
    {
        public string Host { get; set; } = "";

        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";
    }
}
