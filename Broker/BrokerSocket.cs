using Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broker
{
    class BrokerSocket
    {
        private Socket _socket;
        private const int CONNECTIONS_LIMIT = 15;
        public BrokerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

            //spunem socketului sa asculte conexiuni noi
            _socket.Listen(CONNECTIONS_LIMIT);

            //aceptam conexiuni noi
            Accept();
        }

        private void Accept()
        {

            //operatie asincrona BeginAccept
            _socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.Socket = _socket.EndAccept(asyncResult);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None,
                    ReceiveCallback, connection);

            }
            catch(Exception e)
            {
                Console.WriteLine($"It cannot be accepted. {e.Message}");
            }
            finally
            {
                // Acceptam conexiuni noi, se proceseaza asincron
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;
            try
            {
                Socket senderSocket = connection.Socket;
                // vom pastra rezultat returnat daca s-a citit cu succes
                SocketError response;
                int buffSize = senderSocket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payload = new byte[buffSize];
                    // aici se copie toate datele din stream
                    Array.Copy(connection.Data, payload, payload.Length);

                    PayloadHandler.Handle(payload, connection);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"We cannot receive buffer data: {e.Message}");
            }
            // dupa ce am citit datele din o conexiune trebuie sa ascultam date noi
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length,
                        SocketFlags.None, ReceiveCallback, connection);
                }
                catch(Exception e)
                {
                    // cand un socket s-a conectat, consulam, si deconectam
                    Console.Write($"{e.Message}");
                    var address = connection.Socket.RemoteEndPoint.ToString();

                    // stergem din storage
                    ConnectionStorage.Remove(address);

                    connection.Socket.Close();
                    

                }
            }
        }
    }
}
