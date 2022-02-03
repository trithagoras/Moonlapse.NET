using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MoonlapseClient.Networking
{
    public class NetworkController
    {
        TcpClient _client;

        public string Hostname { get; private set; }
        public int Port { get; private set; }

        public NetworkController(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
            _client = new TcpClient();
        }

        public async Task Start()
        {
            await _client.ConnectAsync(Hostname, Port);

            var stream = _client.GetStream();
            var sr = new StreamReader(stream);

            // main read loop
            while (true)
            {
                string s = await sr.ReadLineAsync();
                Console.WriteLine(s);
            }
        }
    }
}
