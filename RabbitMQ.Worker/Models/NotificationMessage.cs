using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Worker.Models
{
    //این کلاس برای این است که json دریافتی از پرودیوسر را از طریق این کلاس دوباره به ابجکت تبدیل کنیم
    public class NotificationMessage
    {
        public string FullName { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string Email { get; set; } = "";
    }
}
