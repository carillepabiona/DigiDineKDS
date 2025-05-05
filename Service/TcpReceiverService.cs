using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DigiDineKDS.Models;

namespace DigiDineKDS.Service
{
    public class TcpReceiverService
    {
        private TcpListener _listener;
        public event Action<List<OrderItem>> OnOrdersReceived;

        public void StartListening(int port = 9000)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            Task.Run(async () =>
            {
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            });
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using var stream = client.GetStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var json = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            var orders = JsonSerializer.Deserialize<List<OrderItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (orders != null && orders.Any())
            {
                OnOrdersReceived?.Invoke(orders);
            }
        }

        public void Stop()
        {
            _listener?.Stop();
        }
    }
}
