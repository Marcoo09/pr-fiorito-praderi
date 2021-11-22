using System;
namespace ServerGrpc
{
    public class ServerConfiguration
    {
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string RabbitMQServerIP { get; set; }
        public string RabbitMQServerPort { get; set; }
        public string LogsQueueName { get; set; }
        public string GrpcApiHttpPort { get; set; }
        public string GrpcApiHttpsPort { get; set; }
    }
}
