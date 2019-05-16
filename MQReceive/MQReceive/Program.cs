using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace MQReceive
{
    class Program
    {
        private static string MQQueueName = "hello1";
        private static string MQHostName = "127.0.0.1";
        private static string MQUserName = "guest";
        private static string MQPassword = "guest";
        private static string ServiceName = "MQ消化";
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<MQComsumer>(s =>
                {
                    s.ConstructUsing(
                        name =>
                            new MQComsumer(MQQueueName, MQHostName, MQUserName, MQPassword));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription(ServiceName + "1");
                x.SetDisplayName(ServiceName + "2");
                x.SetServiceName(ServiceName + "3");
            });
            //var comsumer = new MQComsumer("hello1", "127.0.0.1", "guest", "guest");
            //comsumer.Start();
        }
    }
}
