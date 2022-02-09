using System;
using SadConsole;
using MoonlapseClient.States;
using MoonlapseNetworking.Models.Components;
using MoonlapseNetworking.Models;

namespace MoonlapseClient.Consoles
{
    public class GameConsole : SadConsole.Console
    {
        readonly GameState _state;

        public GameConsole(GameState state) : base(Game.Width, Game.Height, FontController.GameFont)
        {
            _state = state;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            base.Draw(timeElapsed);
            Clear();

            DrawTerrain();

            // todo: draw all entities

            // draw player last
            SetGlyph((Game.Width / 8) - 1, (Game.Height / 4) - 1, 77);
        }

        void DrawTerrain()
        {
            if (_state.PlayerEntity == null || _state.PlayerEntity.GetComponent<Position>() == null)
            {
                return;
            }
            var playerX = _state.PlayerEntity.GetComponent<Position>().X;
            var playerY = _state.PlayerEntity.GetComponent<Position>().Y;

            var viewRadius = 10;

            for (int x = -viewRadius; x < viewRadius + 1; x++)
            {
                for (int y = -viewRadius; y < viewRadius + 1; y++)
                {
                    var xx = playerX + x;
                    var yy = playerY + y;

                    if (!_state.Room.CoordinateExists(xx, yy))
                    {
                        continue;
                    }

                    int halfWidth = (Game.Width / 8) - 1;
                    int halfHeight = (Game.Height / 4) - 1;
                    int cx = x + halfWidth;
                    int cy = y + halfHeight;

                    var pixel = _state.Room.Terrain[yy, xx];
                    SetGlyph(cx, cy, pixel);
                }
            }
        }
    }
}
