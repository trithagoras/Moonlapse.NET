using System.Threading.Tasks;

namespace MoonlapseClient
{
    public class Game
    {
        public const int Width = 80;
        public const int Height = 32;

        public NetworkController NetworkController { get; private set; }

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

        void Init()
        {
            FontController.Init();

            NetworkController = new NetworkController("127.0.0.1", 42523);
            Task.Run(NetworkController.Start);
        }
    }
}