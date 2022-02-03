using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace MoonlapseClient
{
    public class Game
    {
        public const int Width = 80;
        public const int Height = 25;

        public FontController FontController { get; private set; }

        Console _currentConsole;

        public Game()
        {
            FontController = new FontController();
        }

        public void Start()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private void Init()
        {
            FontController.Init();

            var console = new Console(80, 25, FontController.GameFont);
            console.FillWithRandomGarbage();
            console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
            console.Print(4, 4, "Hello from SadConsole");

            SadConsole.Global.CurrentScreen = console;
        }
    }
}