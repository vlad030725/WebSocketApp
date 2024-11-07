using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        string host = "localhost";
        int port = 8080;

        //Запуск сервера
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://{host}:{port}/");
        listener.Start();
        Console.WriteLine($"WebSocket server started at ws://{host}:{port}/");

        while (true)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    Console.WriteLine("Client connected.");
                    _ = Task.Run(() => HandleServerConnection(webSocketContext.WebSocket));
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

    private static async Task HandleServerConnection(WebSocket webSocket)
    {
        while (true)
        {
            Console.Write("Enter a message to send: ");
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) continue;

            if (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Client is not connected. Waiting for a new client connection...");
                break;
            }
        }

        Console.WriteLine("Client disconnected.");
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        webSocket.Dispose();
    }
}
