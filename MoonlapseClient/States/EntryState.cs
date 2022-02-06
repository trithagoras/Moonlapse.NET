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

            Global.CurrentScreen = _console;
        }

        void EntryState_DenyPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<DenyPacket>(args.PacketString);
            ErrorMessage = p.Message;
            _console.SetErrorLabel(ErrorMessage);
        }

        public async Task Login(LoginPacket p)
        {
            await _networkController.SendLine(p.ToString());
        }
    }
}
