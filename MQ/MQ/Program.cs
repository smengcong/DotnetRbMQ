using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQ
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(write1).Start();
            new Thread(write1).Start();
            new Thread(write1).Start();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void write1()
        {
            var factory = new ConnectionFactory() { HostName = "127.0.0.1", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello1", durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                //channel.QueueBind("hello1", "mcExchange", "hello1");
                var props = channel.CreateBasicProperties();
                //for (int i = 0; i < 10000; i++)
                //{
                //    string message = "Hello World ws!" + i;
                //    var body = Encoding.UTF8.GetBytes(message);
                //    channel.BasicPublish("",
                //                  routingKey: "hello1",
                //                  basicProperties: props,
                //                  body: body);
                //    Console.WriteLine(" [x] Sent {0}", message);
                //}
                while (true)
                {
                    var a = Console.ReadLine().ToString();
                    var body = Encoding.UTF8.GetBytes(a);
                    channel.BasicPublish("",
                                  routingKey: "hello1",
                                  basicProperties: props,
                                  body: body);
                    Console.WriteLine("Sent: {0}", a);
                }

            }

            //Console.ReadKey();
        }
    }
}
