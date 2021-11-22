namespace Logger
{
    public class LoggerConfiguration
    {
        public string LogsQueueName { get; set; }
        public string WebApiHttpPort { get; set; }
        public string WebApiHttpsPort { get; set; }
        public string RabbitMQServerIP { get; set; }
        public string RabbitMQServerPort { get; set; }
    }
}