using RabbitMQ.Application.DTOs;
using RabbitMQ.Application.Interface;
using RabbitMQ.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Application.Service
{
    public class UserService : IUserService
    {
        //UserService اصلاً خبر ندارد RabbitMQ چیست.
        //فقط می‌گوید این Message را بفرست

        private readonly IRabbitProducer _producer;

        public UserService(IRabbitProducer producer)
        {
            _producer = producer;
        }

        public async Task RegisterAsync(RegisterVm model)
        {
            //نکته مهم
            //میتونستم RegisterVm را به Rabbit پاس بدیم و دیگه NotificationMessage نباشه
            // ولی اولا RegisterVm ویو است دوما ممکنه چیزای دیگه ای مثل رمز توش باشه که نیازی بهش نداریم

            // Save User

            await _producer.PublishAsync(
                exchange: "notification-exchange",
                queue: "sms-queue",
                routingKey: "sms",
                message: new NotificationMessage
                {
                    FullName = model.FullName,
                    Mobile = model.Mobile,
                    Email = model.Email
                });

            await _producer.PublishAsync(
                exchange: "notification-exchange",
                queue: "email-queue",
                routingKey: "email",
                message: new NotificationMessage
                {
                    FullName = model.FullName,
                    Mobile = model.Mobile,
                    Email = model.Email
                });
        }
    }
}
