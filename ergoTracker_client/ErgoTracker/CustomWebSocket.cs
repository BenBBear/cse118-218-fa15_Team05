using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ErgoTracker
{
    class CustomWebSocket
    {
        string urlToConnect = "ws://maxubi.herokuapp.com/";
        UTF8Encoding encoding = new UTF8Encoding();
        ClientWebSocket aClient;

        public CustomWebSocket()
        {
            ConnectToServer().Wait();
        }

        ~CustomWebSocket()
        {
            aClient.Dispose();
        }

        public async Task ConnectToServer()
        {
            Thread.Sleep(1000);

            try
            {
                aClient = new ClientWebSocket();
                await aClient.ConnectAsync(new Uri(urlToConnect), CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Web Socket did not open.");
            }
            finally
            {
                if (aClient.State == WebSocketState.Open)
                    Console.WriteLine("Web Socket is now open");
            }
        }

        public async Task SendJsonString(string json)
        {
            if (aClient.State != WebSocketState.Open)
            {
                Console.WriteLine("WebSocket is not open! Aborting!");
            }
            else
            {
                byte[] encoded = encoding.GetBytes(json);
                ArraySegment<Byte> buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                await aClient.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                Console.WriteLine("Sending...");

                await Task.Delay(1000);
            }
        }
    }
}
