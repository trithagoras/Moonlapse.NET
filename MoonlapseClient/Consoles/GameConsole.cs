using System;
using SadConsole;
using MoonlapseClient.States;

namespace MoonlapseClient.Consoles
{
    public class GameConsole : SadConsole.Console
    {
        readonly GameState _state;

        public GameConsole(GameState state) : base(Game.Width, Game.Height, FontController.GameFont)
        {
            _state = state;
            FillWithRandomGarbage();
        }
    }
}
