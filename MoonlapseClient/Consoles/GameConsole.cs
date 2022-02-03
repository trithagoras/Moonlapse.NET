using System;
using SadConsole;
namespace MoonlapseClient.Consoles
{
    public class GameConsole : SadConsole.Console
    {
        Game _game;

        public GameConsole(Game game) : base(Game.Width, Game.Height, game.FontController.GameFont)
        {
            _game = game;
        }

        public void Draw()
        {

        }
    }
}
