// WebSocketClient/Program.cs
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    private static readonly int ReconnectDelaySeconds = 5;

    public static async Task Main()
    {
        string host = "localhost"; // Адрес сервера
        int port = 8080;           // Порт сервера

        while (true)
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {
                Uri serverUri = new Uri($"ws://{host}:{port}/");

                try
                {
                    Console.WriteLine($"Connecting to server at {serverUri}");
                    await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                    Console.WriteLine("Connected to server.");

                    await ReceiveMessages(webSocket);
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}. Retrying in {ReconnectDelaySeconds} seconds...");
                    await Task.Delay(ReconnectDelaySeconds * 1000);
                }
            }
        }
    }

    private static async Task ReceiveMessages(ClientWebSocket webSocket)
    {
        byte[] buffer = new byte[1024];
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
                break;
            }
        }
    }
}
