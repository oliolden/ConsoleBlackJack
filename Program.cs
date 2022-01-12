using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Resources;
using System.Diagnostics;

namespace Black_Jack {
    class Program {
        private static string path = "chips.txt";
        public static long chips;
        private static DateTime chipsTime;
        private static TimeSpan chipsTimer;
        public static List<int> OwnedSkins = new List<int>();
        public static int EquippedSkin;
        public static Graphics graphics;

        public static void Save() {
            string time;
            string[] readlines;
            try {
                readlines = File.ReadAllLines(path);
            }
            catch (Exception) {
                readlines = new string[4];
            }
            if (readlines[1] == "" && chips == 0) {
                time = DateTime.Now.AddMinutes(5).ToString();
                chipsTime = Convert.ToDateTime(time);
            }
            else if (chips == 0)
                time = readlines[1];
            else
                time = "";
            string SkinsString = "";
            foreach (int skin in OwnedSkins) {
                SkinsString += skin.ToString() + " ";
            }
            string[] lines = new string[] {
                chips.ToString(),
                time,
                SkinsString,
                EquippedSkin.ToString()
            };
            File.WriteAllLines(path, lines);
        }

        public static void Load() {
            string[] lines;
            try {
                lines = File.ReadAllLines(path);
            }
            catch (Exception) {
                lines = new string[] {
                    "0", "", "0", "0"
                };
            }
            // Load Chips.
            try {
                chips = Convert.ToInt32(lines[0]);
                try {
                    chipsTime = Convert.ToDateTime(lines[1]);
                }
                catch (Exception) {
                    chipsTime = new DateTime();
                }
            }
            catch (Exception) {
                chips = 0;
            }
            // Load Skins.
            try {
                foreach (string skin in lines[2].Trim().Split()) {
                    OwnedSkins.Add(Convert.ToInt32(skin));
                }
            }
            catch (Exception) {
                OwnedSkins = new List<int> {
                    0
                };
            }
            // Load Equipped Skin.
            try {
                EquippedSkin = Convert.ToInt32(lines[3]);
            }
            catch (Exception) {
                EquippedSkin = 0;
            }
            return;
        }

        public static void ClearLine(int line) {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth - 1));
        }

        public static void ClearRectangle(int x, int y, int width, int height) {
            string write = new string(' ', width);
            for (int i = 0; i < height; i++) {
                Console.SetCursorPosition(x, y + i);
                Console.Write(write);
            }
        }

        public static void RenderChips() {
            UpdateChips();
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($"Chips: {chips}" + new string(' ', 10));
            if (chips == 0) {
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.Write($"You get more chips in {chipsTimer:mm}:{chipsTimer:ss}.");
            }
        }

        static void UpdateChips() {
            if (chips == 0) {
                if (chipsTime.CompareTo(DateTime.Now) == -1) { Save(); }
                if (chipsTime.CompareTo(DateTime.Now) == -1) {
                    Graphics.clear = true;
                    chips = 50;
                    Save();
                }
                else {
                    chipsTimer = chipsTime.Subtract(DateTime.Now);
                }
            }
        }

        static void WindowListener() {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            while (true) {
                try {
                    while (width != Console.WindowWidth || height != Console.WindowHeight) {
                        width = Console.WindowWidth;
                        height = Console.WindowHeight;
                        Console.BufferHeight = Console.WindowHeight;
                        Graphics.clear = true;
                    }
                }
                catch (ArgumentOutOfRangeException) {
                    Graphics.clear = true;
                    continue;
                }
            }

        }

        static void Main(string[] args) {
            Load();
            Console.BufferHeight = Console.WindowHeight;
            Thread WindowListenerThread = new Thread(new ThreadStart(WindowListener));
            WindowListenerThread.Name = "WindowListenerThread";
            WindowListenerThread.Start();
            graphics = new Graphics();
            MainMenu menu = new MainMenu();
        }
    }
}
