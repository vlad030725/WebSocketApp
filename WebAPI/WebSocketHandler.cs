using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using WebSocketServer.BLL.Interfaces;
using WebSocketServer.BLL.DTO;

namespace WebAPI
{
    public class WebSocketHandler
    {
        private static readonly ConcurrentBag<WebSocket> Clients = new ConcurrentBag<WebSocket>();
        private readonly IMessageService _messageService;

        public WebSocketHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

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
                        _messageService.CreateMessage(new MessageDTO()
                        {
                            Content = message,
                            ConnectionId = client.GetHashCode().ToString(),
                        });
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

        public async Task HandleWebSocketAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Client connected.");
                AddClient(webSocket);

                try
                {
                    byte[] buffer = new byte[1024];
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            break;
                        }

                        // Обработка входящего сообщения (пример вывода в консоль)
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received: {message}");
                    }
                }
                catch
                {
                    Console.WriteLine("WebSocket error.");
                }
                finally
                {
                    RemoveClient(webSocket);
                    Console.WriteLine("Client disconnected.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                Console.WriteLine("Bad Request: Not a WebSocket request.");
            }
        }
    }
}
