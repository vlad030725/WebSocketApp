// WebSocketServer/Program.cs
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    private static readonly ConcurrentBag<WebSocket> Clients = new ConcurrentBag<WebSocket>();

    public static async Task Main()
    {
        string host = "localhost"; // Адрес сервера
        int port = 8080;           // Порт сервера

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://{host}:{port}/");
        listener.Start();
        Console.WriteLine($"WebSocket server started at ws://{host}:{port}/");

        // Запускаем обработку пользовательского ввода сообщений
        _ = Task.Run(() => HandleServerMessages());

        while (true)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    Console.WriteLine("Client connected.");
                    Clients.Add(webSocketContext.WebSocket);
                    _ = Task.Run(() => MonitorClient(webSocketContext.WebSocket));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
        }
    }

    private static async Task MonitorClient(WebSocket webSocket)
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                // Пустой цикл для поддержания соединения клиента
                await Task.Delay(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client monitoring error: {ex.Message}");
        }
        finally
        {
            RemoveClient(webSocket);
            Console.WriteLine("Client disconnected.");
        }
    }

    private static void RemoveClient(WebSocket webSocket)
    {
        Clients.TryTake(out _);
        webSocket.Dispose();
    }

    private static async Task BroadcastMessage(string message)
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send message to a client: {ex.Message}");
                }
            }
            else
            {
                RemoveClient(client);
            }
        }
    }

    private static async Task HandleServerMessages()
    {
        while (true)
        {
            Console.Write("Enter a message to send to all clients: ");
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) continue;

            await BroadcastMessage(message);
        }
    }
}
