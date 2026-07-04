using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Application.ViewModels
{
    public class RegisterVm
    {
        public string FullName { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string Email { get; set; } = "";
    }
}
