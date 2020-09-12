using Shared;
using System;
using System.Threading.Tasks;

namespace Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's me Broker");

            BrokerSocket socket = new BrokerSocket();
            socket.Start(Settings.BROKER_IP, Settings.BROKER_PORT);

            var messageWorker = new MessageWorker();
            Task.Factory.StartNew(messageWorker.DoSendMessageWork, TaskCreationOptions.LongRunning);
            
            Console.ReadLine();


        }
    }
}
