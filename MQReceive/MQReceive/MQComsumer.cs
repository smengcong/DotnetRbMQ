using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQReceive
{
    public class MQComsumer
    {
        private string _QueueName;
        private string _QueueIp;
        private string _QueueUserName;
        private string _QueuePwd;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        public MQComsumer(string QueueName,string QueueIp,string QueueUserName,string QueuePwd)
        {
            _QueueName = QueueName;
            _QueueIp = QueueIp;
            _QueueUserName = QueueUserName;
            _QueuePwd = QueuePwd;
        }

        public void Start()
        {
            _factory = new ConnectionFactory() { HostName = _QueueIp, UserName = _QueueUserName, Password = _QueuePwd };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Thread.Sleep(2000);
                Console.WriteLine(message + "已接收");
                WriteLog(message);
                //手动应答 需要确认消息
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(queue: _QueueName,
                                 autoAck: false,//自动应答
                                 consumer: consumer);

        }

        public void Stop()
        {
            _channel.Close();
            _connection.Close();
        }

        public static void WriteLog(string i)
        {
            using (FileStream f = new FileStream(@"A.txt", FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(f, Encoding.Default))
                {
                    sw.Write(i + "\n");
                }
            }
        }
    }
}
