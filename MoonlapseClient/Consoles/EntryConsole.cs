using System;
using Microsoft.Xna.Framework;

namespace MoonlapseClient.Consoles
{
    public class EntryConsole : SadConsole.Console
    {

        Game _game;

        public EntryConsole(Game game) : base(Game.Width, Game.Height, game.FontController.TextFont)
        {
            _game = game;
        }

        public void Draw()
        {
            //FillWithRandomGarbage();
            Print(4, 4, "Moonlapse");

            Print(4, 6, "Username: ");
            Fill(new Rectangle(15, 6, 23, 3), Color.Cyan, Color.Black, 0, 0);

            Print(4, 7, "Password: ");
            Fill(new Rectangle(15, 7, 23, 3), Color.Cyan, Color.Black, 0, 0);
        }
    }
}
