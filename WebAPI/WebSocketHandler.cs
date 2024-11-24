using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebAPI
{
    public class WebSocketHandler
    {
        private static readonly ConcurrentBag<WebSocket> Clients = new ConcurrentBag<WebSocket>();

        public void AddClient(WebSocket webSocket)
        {
            Clients.Add(webSocket);
        }

        public void RemoveClient(WebSocket webSocket)
        {
            Clients.TryTake(out _);
        }

        public async Task BroadcastMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (var client in Clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    try
                    {
                        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        RemoveClient(client);
                    }
                }
                else
                {
                    RemoveClient(client);
                }
            }
        }
    }
}
