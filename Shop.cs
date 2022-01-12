using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Black_Jack {
    class Shop : Scene {
        private bool closing;
        private int scroll;
        private bool cantAfford;
        // Price array using same index as CardVisuals.Skins
        static int[] Prices = new int[] {
            0, 100, 200, 300, 400
        };

        // Method for rendering the shop.
        public override void Render() {
            Console.CursorVisible = false;
            // Title.
            string title = "Shop:";
            Console.SetCursorPosition((Console.WindowWidth / 2) - (title.Length / 2), (Console.WindowHeight / 2) - 6);
            Console.Write(title);
            // Shop items with selection in center.
            for (int i = 0; i < CardVisuals.Skins.Count(); i++) {
                double pos = i - scroll;
                CardVisuals.Skins[i].Draw((Console.WindowWidth / 2) + (Convert.ToInt32(10 * pos) - 3), (Console.WindowHeight / 2) - 4);
            }
            // Info
            string info = "";
            if (scroll == Convert.ToInt32(scroll)) {
                if (Program.EquippedSkin == Convert.ToInt32(scroll)) { info = "  Equipped  "; }
                else if (Program.OwnedSkins.Contains(Convert.ToInt32(scroll))) { info = "    Owned    "; }
                else { info = Prices[Convert.ToInt32(scroll)].ToString() + " chips"; }
                Console.SetCursorPosition((Console.WindowWidth / 2) - (info.Length / 2), (Console.WindowHeight / 2) + 8);
                Console.Write(info);
            }
            else
                Program.ClearLine((Console.WindowHeight / 2) + 8);
            if (cantAfford) {
                string print = "Insufficient funds.";
                Console.SetCursorPosition((Console.WindowWidth / 2) - (print.Length / 2), (Console.WindowHeight / 2) + 7);
                Console.Write(print);
            }
            else { Program.ClearLine((Console.WindowHeight / 2) + 7); }
            Program.RenderChips();
        }

        // Method for handling inputs.
        private void HandleInputs() {
            if (Console.KeyAvailable) {
                ConsoleKey key = Console.ReadKey(true).Key;
                cantAfford = false;
                switch (key) {
                    case ConsoleKey.Escape:
                        closing = true;
                        break;
                    case ConsoleKey.RightArrow:
                        if (scroll < CardVisuals.Skins.Count() - 1) {
                            scroll += 1;
                            Graphics.clear = true;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (scroll > 0) {
                            scroll -= 1;
                            Graphics.clear = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (Program.OwnedSkins.Contains(Convert.ToInt32(scroll))) {
                            Program.EquippedSkin = Convert.ToInt32(scroll);
                            Program.Save();
                        }
                        else if (Program.chips > Prices[Convert.ToInt32(scroll)]) {
                            Program.chips -= Prices[Convert.ToInt32(scroll)];
                            Program.OwnedSkins.Add(Convert.ToInt32(scroll));
                            Program.EquippedSkin = Convert.ToInt32(scroll);
                            Program.Save();
                        }
                        else { cantAfford = true; }
                        break;
                }
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            }
        }

        // Method for opening the shop.
        public Shop() {
            Console.Clear();
            closing = false;
            scroll = Program.EquippedSkin;
            Program.graphics.CurrentScene = this;
            // Main shop loop.
            while (!closing) {
                HandleInputs();
            }
        }
    }
}
