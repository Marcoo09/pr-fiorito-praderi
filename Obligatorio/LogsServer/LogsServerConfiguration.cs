using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsServer
{
    public class LogsServerConfiguration
    {
        public string RabbitMQServerIP { get; set; }
        public string RabbitMQServerPort { get; set; }
        public string LogsQueueName { get; set; }
        public string WebApiHttpPort { get; set; }
        public string WebApiHttpsPort { get; set; }
    }
}
