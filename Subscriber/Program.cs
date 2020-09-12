using Shared;
using System;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's me Subsriber");

            string topic;

            Console.Write("What is the Topic: ");
            topic = Console.ReadLine().ToLower();

            var subscriberSocket = new SubscriberSocket(topic);

            subscriberSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            Console.WriteLine("To exit press any key");
            Console.ReadLine();
        }
    }
}
