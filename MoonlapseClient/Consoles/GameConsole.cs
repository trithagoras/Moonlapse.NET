using System;
using SadConsole;
using MoonlapseClient.UserStates;
using MoonlapseNetworking.Models.Components;
using MoonlapseNetworking.Models;

namespace MoonlapseClient.Consoles
{
    public class GameConsole : SadConsole.Console
    {
        readonly GameState _userState;

        public GameConsole(GameState userState) : base(Game.Width, Game.Height, FontController.GameFont)
        {
            _userState = userState;
            IsCursorDisabled = true;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            base.Draw(timeElapsed);
            Clear();

            DrawTerrain();

            // todo: draw all entities
            DrawEntities();

            // draw player last
            SetGlyph((Game.Width / 8) - 1, (Game.Height / 4) - 1, 77);
        }

        void DrawEntities()
        {
            if (_userState.PlayerEntity == null)
            {
                return;
            }

            foreach (var entity in _userState.KnownEntities.Values)
            {
                if (entity == _userState.PlayerEntity)
                {
                    continue;
                }

                var pPos = _userState.PlayerEntity.GetComponent<Position>();
                var ePos = entity.GetComponent<Position>();

                // either entity has no position component or not loaded yet
                if (pPos == null || ePos == null)
                {
                    continue;
                }

                // Draw other players on top of all else
                if (entity.TypeName == "Player")
                {
                    int halfWidth = (Game.Width / 8) - 1;
                    int halfHeight = (Game.Height / 4) - 1;

                    // todo: check viewport

                    var cx = halfWidth + ePos.X - pPos.X;
                    var cy = halfHeight + ePos.Y - pPos.Y;

                    SetGlyph(cx, cy, 77);
                }
            }
        }

        void DrawTerrain()
        {
            if (_userState.PlayerEntity == null || _userState.PlayerEntity.GetComponent<Position>() == null)
            {
                return;
            }
            var playerX = _userState.PlayerEntity.GetComponent<Position>().X;
            var playerY = _userState.PlayerEntity.GetComponent<Position>().Y;
            var room = _userState.PlayerEntity.GetComponent<Position>().Room;

            var viewRadius = 10;

            for (int x = -viewRadius; x < viewRadius + 1; x++)
            {
                for (int y = -viewRadius; y < viewRadius + 1; y++)
                {
                    var xx = playerX + x;
                    var yy = playerY + y;

                    if (!room.CoordinateExists(xx, yy))
                    {
                        continue;
                    }

                    int halfWidth = (Game.Width / 8) - 1;
                    int halfHeight = (Game.Height / 4) - 1;
                    int cx = x + halfWidth;
                    int cy = y + halfHeight;

                    var pixel = room.Terrain[yy, xx];
                    SetGlyph(cx, cy, pixel);
                }
            }
        }
    }
}
