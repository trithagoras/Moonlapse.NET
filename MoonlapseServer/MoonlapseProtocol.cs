using System;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using MoonlapseServer.States;
using MoonlapseNetworking;
using MoonlapseServer.Utils.Logging;

namespace MoonlapseServer
{
    public class Protocol
    {
        readonly TcpClient _client;
        readonly Server _server;

        public State State { get; set; }

        public string Username { get; private set; }

        public Protocol(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
            State = new EntryState(this);
        }

        public async Task Start()
        {
            var stream = _client.GetStream();
            var sr = new StreamReader(stream);

            // main read loop
            string line;
            try
            {
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    StringRecieved(line);
                }
            } catch (Exception)
            {
            }

            // connection ended
            ConnectionEnded();
        }

        public void Login(string username)
        {
            Username = username;
            Log("Successfully logged in");
        }

        void StringRecieved(string s)
        {
            try
            {
                State.HandlePacketFromString(s);
            }
            catch (PacketEventNotSubscribedException e)
            {
                Log(e.Message, LogContext.Warn);
            }
            catch (Exception e)
            {
                Log($"Malformed message received: {s} - inner details: {e.Message}", LogContext.Error);
            }
        }

        void ConnectionEnded()
        {
            Log("Client disconnected");
            _server.ConnectedProtocols.Remove(this);
        }

        public void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log(Username ?? "None", message, context);
        }
    }
}
