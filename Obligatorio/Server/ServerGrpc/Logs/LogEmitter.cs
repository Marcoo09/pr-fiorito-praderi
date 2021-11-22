using System;
using System.Text;
using Logger.Domain;
using RabbitMQ.Client;

namespace ServerGrpc.Logs
{
    public class LogEmitter
    {
        private IModel _channel;
        private string _queueName;

        public LogEmitter(ServerConfiguration configuration)
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
        }

        public void EmitLog(string logMessage, Tag tag)
        {
            string messageToSend = $"{DateTime.Now.Ticks}||{tag}||{logMessage}";
            byte[] body = Encoding.UTF8.GetBytes(messageToSend);

            _channel.BasicPublish("", _queueName, null, body);
        }
    }
}