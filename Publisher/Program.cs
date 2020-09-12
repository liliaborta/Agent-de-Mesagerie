using Newtonsoft.Json;
using Shared;
using System;
using System.Text;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's me Publisher ");

            var publisherSocket = new PublisherSocket();
            // noi vom utliza o librarie comuna utilizata si de sender si broker, publicher, 
            // in caz de e schimbata adresa la broker sa nu fie nevoie sa schimbam si la sender si publisher in toate proiectele
            publisherSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            if (publisherSocket.IsConnected) 
            {
                while (true)
                {
                    var payload = new Payload();

                    // noi citim de la tastatura mesajele si topicurile
                    Console.Write("What is the Topic :");
                    payload.Topic = Console.ReadLine().ToLower();

                    Console.Write("What is the Message :");
                    payload.Message = Console.ReadLine();

                    var payloadString = JsonConvert.SerializeObject(payload);
                    byte[] data = Encoding.UTF8.GetBytes(payloadString);

                    publisherSocket.Send(data);

                }
            }
           
            Console.ReadLine();
        }
    }
}
