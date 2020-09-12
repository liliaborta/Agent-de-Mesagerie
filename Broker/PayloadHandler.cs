using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broker
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("subscribe#"))
            {
                connectionInfo.Topic = payloadString.Split("subscribe#").LastOrDefault();
                // adaugam conexiunea in storage 
                ConnectionStorage.Add(connectionInfo);
            }
            else
            {
                // il facem transient
                Payload payload = JsonConvert.DeserializeObject<Payload>(payloadString);
                // adaugam in storage
                PayloadStorage.Add(payload);
             
            }
             
            // Console.WriteLine(payloadString);
        }
    }
}
