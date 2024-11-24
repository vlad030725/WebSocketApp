using System;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

public class Program
{
    private static readonly int ReconnectDelaySeconds = 5;

    public static async Task Main()
    {
        int port = 7126;

        while (true)
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {
                Uri serverUri = new Uri($"wss://localhost:{port}/ws");

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                try
                {
                    Console.WriteLine($"Connecting to server at {serverUri}...");
                    await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                    Console.WriteLine("Connected to server.");

                    await ReceiveMessages(webSocket);
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}. Retrying in {ReconnectDelaySeconds} seconds...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}. Retrying in {ReconnectDelaySeconds} seconds...");
                }
                finally
                {
                    Console.WriteLine("Disconnected. Attempting to reconnect...");
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
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Server closed the connection.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                break;
            }
        }
    }
}
