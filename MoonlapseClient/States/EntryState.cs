using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseClient.Consoles;
using System.Threading.Tasks;
using SadConsole;

namespace MoonlapseClient.States
{
    public class EntryState : State
    {
        readonly EntryConsole _console;
        readonly NetworkController _networkController;

        public string ErrorMessage { get; set; }

        public EntryState(NetworkController nc)
        {
            _console = new EntryConsole(this)
            {
                IsFocused = true
            };
            _networkController = nc;

            DenyPacketEvent += EntryState_DenyPacketEvent;
            OkPacketEvent += EntryState_OkPacketEvent;

            Global.CurrentScreen = _console;
        }

        private void EntryState_OkPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<OkPacket>(args.PacketString);

            if (p.Message == "Login")
            {
                _networkController.CurrentState = new GameState(_networkController);
            }
            else if (p.Message == "Register")
            {
                _console.SetErrorLabel("Registration Successful", false);
            }
        }

        void EntryState_DenyPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<DenyPacket>(args.PacketString);
            ErrorMessage = p.Message;
            _console.SetErrorLabel(ErrorMessage);
        }

        public void Register(RegisterPacket p)
        {
            _networkController.SendPacket(p);
        }

        public void Login(LoginPacket p)
        {
            _networkController.SendPacket(p);
        }
    }
}
