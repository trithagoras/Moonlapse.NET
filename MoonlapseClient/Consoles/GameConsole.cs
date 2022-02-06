using System;
using SadConsole;
using MoonlapseClient.States;
using Microsoft.Xna.Framework;

namespace MoonlapseClient.Consoles
{
    public class GameConsole : SadConsole.Console
    {
        readonly GameState _state;

        public GameConsole(GameState state) : base(Game.Width, Game.Height, FontController.GameFont)
        {
            _state = state;
            //FillWithRandomGarbage();
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            base.Draw(timeElapsed);

            SetGlyph((Game.Width / 8) - 1, (Game.Height / 4) - 1, 77, Color.Wheat);
        }
    }
}
