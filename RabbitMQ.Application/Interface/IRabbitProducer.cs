using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Application.Interface
{
    public interface IRabbitProducer
    {
        Task PublishAsync<T>(string exchange, string queue, string routingKey,T message);
        //یا
        //Task PublishAsync(NotificationMessage message);
        //چرا جنریک نوشتیم
        //چون فقط Notification می‌توانستیم بفرستیم برای همین جنریک نوشتیم
        //شاید بعدا PaymentMessage هم خواستیم بنویسیم
    }
}
