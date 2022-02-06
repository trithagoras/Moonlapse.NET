using System;
using MoonlapseNetworking;
using MoonlapseClient.Consoles;
using SadConsole;

namespace MoonlapseClient.States
{
    public class GameState : State
    {
        readonly GameConsole _console;
        readonly NetworkController _networkController;

        public GameState(NetworkController nc)
        {
            _networkController = nc;
            _console = new GameConsole(this)
            {
                IsFocused = true
            };

            Global.CurrentScreen = _console;
        }
    }
}
