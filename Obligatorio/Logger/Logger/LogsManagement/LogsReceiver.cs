using System;
using System.Configuration;
using System.Text;
using Logger.DataAccess.Interfaces;
using Logger.Domain;
using Logger.DataAccess.Implementations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Logger.LogsManagement
{
    public class LogReceiver
    {
        private IModel _channel;
        private string _queueName;
        private ILogRepository _logRepository;
        private LogProcessor _logProcessor;

        public LogReceiver(LoggerConfiguration configuration)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = configuration.RabbitMQServerIP,
                Port = Int32.Parse(configuration.RabbitMQServerPort)
            };
            IConnection connection = connectionFactory.CreateConnection();
            _queueName = configuration.LogsQueueName;
            _channel = connection.CreateModel();
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _logRepository = LogRepository.GetInstance();
            _logProcessor = new LogProcessor();
        }

        public void ReceiveServerLogs()
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                byte[] body = eventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Log processedLog = _logProcessor.ProcessLog(message);

                await _logRepository.StoreAsync(processedLog);
                Console.WriteLine(processedLog.ToString());
            };

            _channel.BasicConsume(_queueName, true, consumer);
        }

    }
}