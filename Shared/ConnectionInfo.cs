using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Shared
{
    public class ConnectionInfo
    {

        public const int BUFF_SIZE = 1024;
        public Socket Socket { get; set; }

        // avem nevoie de adrress ca in caz ca s-a deconectat noi vom elimina aceasta adresa din memorie
        public string Address { get; set; }

        public string Topic { get; set; }

        // buffer cu care va lucara conexiunea noastra

        public byte[] Data { get; set; }

        public ConnectionInfo()
        {
            Data = new byte[BUFF_SIZE];
        }

    }
}
