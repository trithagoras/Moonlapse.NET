using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using MoonlapseClient.Consoles;
using MoonlapseClient.Networking;
using System.Threading.Tasks;

namespace MoonlapseClient
{
    public class Game
    {
        public const int Width = 80;
        public const int Height = 25;

        public FontController FontController { get; private set; }

        public NetworkController NetworkController { get; private set; }

        Console _currentConsole;

        public Game()
        {
            FontController = new FontController();
            NetworkController = new NetworkController("127.0.0.1", 42523);
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
            _currentConsole = new EntryConsole(this);

            // init networking
            Task.Run(NetworkController.Start);

            Global.CurrentScreen = _currentConsole;
            ((EntryConsole)_currentConsole).Draw();
        }
    }
}