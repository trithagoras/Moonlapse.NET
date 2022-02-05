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

        public string ErrorMessage { get; private set; }

        public NetworkController(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
            _client = new TcpClient();
        }

        public async Task Start()
        {
            try
            {
                await _client.ConnectAsync(Hostname, Port);
            }
            catch (Exception)
            {
                ErrorMessage = "Unable to connect to server";
            }

            var stream = _client.GetStream();
            var sr = new StreamReader(stream);

            // main read loop
            while (true)
            {
                try
                {
                    string s = await sr.ReadLineAsync();

                    // if DenyPacket, ErrorMessage = packet.Message
                    Console.WriteLine(s);
                }
                catch (Exception)
                {
                    ErrorMessage = "Connection to server lost";
                }
                
            }
        }

        public async void SendLine(string s)
        {
            try
            {
                var sw = new StreamWriter(_client.GetStream());
                await sw.WriteLineAsync(s);
                await sw.FlushAsync();
            }
            catch (Exception)
            {
                ErrorMessage = "Connection to server lost";
            }
        }
    }
}
