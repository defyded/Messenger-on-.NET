using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Client
{
    public class MyClient
    {
        string? url { get; set; }

        public async Task StartClient()
        {
            Console.Write("Enter your name: ");
            var user = Console.ReadLine();

            url = "https://localhost:7007/chat";

            var connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string>("ReceiveMessage", (from, message) =>
            {
                var prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] {from}: {message}");
                Console.ForegroundColor = prev;
                Console.WriteLine(">");
            });
            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connected to the server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                return;

            }
            Console.WriteLine(">");
            while (true)
            {
                var text = Console.ReadLine();
                if (string.Equals(text, "/exit", StringComparison.OrdinalIgnoreCase))
                    break;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    try
                    {
                        await connection.InvokeAsync("SendMessage", user, text);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка отправки: " + ex.Message);
                    }
                }
                Console.Write("> ");
            }
        }
    }
}
