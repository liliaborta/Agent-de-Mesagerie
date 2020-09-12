using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Publisher
{
    class PublisherSocket
    {
        private Socket _socket;
        public bool IsConnected;

 //  Initializam metoda PublisherSocket     
        public PublisherSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }
// metoda ce ne permite conexiunea la broker

        public void Connect(string ipAdress, int port)
        {
            //beginConnect varianta asincrona
            //bC porneste un alt fir de executie
            // null - este atribuit la variabila state

            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), port), ConnectedCallback, null);
            Thread.Sleep(3000);

// vom adormi Threadul principal , daca nu se conecteaza in 3 sec, atunci se executa iesire din program
        }

        public void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch(Exception e)
            {
                Console.WriteLine($" This data can not be send. {e.Message}");
            }
        }

        // rezultatul care se transmite in urma unei operatii asincrone
        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            // aici vom verifica daca conexiunea s-a efectuat sau nu
            if (_socket.Connected)
            {
                Console.WriteLine("Our Sender is connected to the Broket hoorray!!!! ");
            } else
            {
                Console.WriteLine("ERROR: Sender not connected to the Broker :( ");

            }

            IsConnected = _socket.Connected;
        }
    }
}
