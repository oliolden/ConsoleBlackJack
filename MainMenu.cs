using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Black_Jack {
    internal class MainMenu : Scene {
        private int Selection = 0;
        private Dictionary<string, Action> Items = new Dictionary<string, Action> {
                { "Play", Play },
                { "Controls", Controls },
                { "Shop", OpenShop },
                { "Credit", Credit },
                { "Quit", Quit }
            };
        public MainMenu() { Start(); }

        private void Start() {
            Console.Clear();
            Console.ResetColor();
            Program.graphics.CurrentScene = this;
            while (true) {
                HandleInputs();
            }
        }
        public override void Render() {
            try {
                Console.CursorVisible = false;

                // Write title with smaller title catch if it doesn't fit.
                if (Console.WindowWidth > 36) {
                    string[] title = new string[] {
                            @" ___ _         _      _         _   ",
                            @"| _ ) |__ _ __| |___ | |__ _ __| |__",
                            @"| _ \ / _` / _| / / || / _` / _| / /",
                            @"|___/_\__,_\__|_\_\\__/\__,_\__|_\_\",
                        };
                    for (int i = 0; i < title.Length; i++) {
                        Console.SetCursorPosition((Console.WindowWidth / 2) - (title[i].Length / 2), (Console.WindowHeight / 2) - (7 - i));
                        Console.Write(title[i]);
                    }
                }
                else {
                    string title = "Blackjack!";
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (title.Length / 2), (Console.WindowHeight / 2) - 4);
                    Console.Write(title);
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (title.Length / 2), (Console.WindowHeight / 2) - 3);
                    Console.Write(new string('¯', title.Length));
                }

                // Iterate and write menu items.
                for (int i = 0; i < Items.Count(); i++) {
                    string name = Items.Keys.ElementAt(i);
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (name.Length / 2), (Console.WindowHeight / 2) - (2 - i));

                    // Reverse colors for selected item.
                    if (i == Selection) {
                        ConsoleColor FGcolor = Console.BackgroundColor;
                        Console.BackgroundColor = Console.ForegroundColor;
                        Console.ForegroundColor = FGcolor;
                    }
                    Console.Write(name);
                    Console.ResetColor();
                }
                Program.RenderChips();
            }
            catch (Exception) { }
        }

        private void HandleInputs() {
            // Handle inputs.
            if (Console.KeyAvailable) {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key) {
                    case ConsoleKey.UpArrow:
                        if (Selection != 0)
                            Selection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (Selection != Items.Count - 1)
                            Selection++;
                        break;
                    case ConsoleKey.Enter:
                        try {
                            Items.Values.ElementAt(Selection)();
                            Program.graphics.CurrentScene = this;
                            while (Console.KeyAvailable) Console.ReadKey(true);
                            break;
                        }
                        catch (Exception) {
                            throw;
                        }
                }
            }
        }
        static void Play() {
            Game MyGame = new Game();
            MyGame.Start();
        }

        class ControlsScene : Scene {
            public ControlsScene() { Program.graphics.CurrentScene = this; }
            public override void Render() {
                try {
                    Console.CursorVisible = false;
                    string row1 = "Space: Hit";
                    string row2 = "Enter: Stay";
                    string row3 = "Press any key to continue...";
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (row1.Length / 2), (Console.WindowHeight / 2) - 2);
                    Console.Write(row1);
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (row2.Length / 2), (Console.WindowHeight / 2));
                    Console.Write(row2);
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (row3.Length / 2), (Console.WindowHeight / 2) + 2);
                    Console.Write(row3);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        static void Controls() {
            new ControlsScene();
            Console.ReadKey(true);
        }

        static void OpenShop() {
            Shop shop = new Shop();
        }

        class Credits : Scene {
            public Credits() { Program.graphics.CurrentScene = this; }

            string credits = "Made by Oliver Oldenstam";
            public float dist = 1.00f;
            public int sleep = 10000 / Console.WindowHeight;


            public override void Render() {
                try {
                    sleep = 10000 / Console.WindowHeight;
                    Console.CursorVisible = false;
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (credits.Length / 2), Convert.ToInt32(Console.WindowHeight * dist) - 2);
                    Console.Write(credits);
                    Program.ClearLine(Convert.ToInt32(Console.WindowHeight * dist) - 1);
                }
                catch { }
            }
        }

        static void Credit() {
            Credits c = new Credits();
            while (Convert.ToInt32(Console.WindowHeight * c.dist) - 2 >= 0) {
                c.dist -= 1f / Console.WindowHeight;
                Thread.Sleep(c.sleep);
                if (Console.KeyAvailable)
                    return;
            }
        }

        static void Quit() {
            Program.Save();
            Environment.Exit(0);
        }
    }
}
