using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Black_Jack {
    static class CardVisuals {
        public static Dictionary<string, string> Names = new Dictionary<string, string> // Prefix, Name.
        {
            { "A", "ace" },
            { "2", "two" },
            { "3", "three" },
            { "4", "four" },
            { "5", "five" },
            { "6", "six" },
            { "7", "seven" },
            { "8", "eight" },
            { "9", "nine" },
            { "10", "ten" },
            { "J", "jack" },
            { "Q", "queen" },
            { "K", "king" }
        };

        public static Dictionary<string, string> Suits = new Dictionary<string, string> // Prefix, Name.
        {
            { "♣", "clubs" }, { "♦", "diamonds" }, { "♥", "hearts" }, { "♠", "spades" }
        };

        public static Skin[] Skins = new Skin[] {
            new Skin(ConsoleColor.DarkRed, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkBlue),
            new Skin(ConsoleColor.DarkRed, ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.DarkYellow),
            new Skin(ConsoleColor.DarkMagenta, ConsoleColor.DarkCyan, ConsoleColor.White, ConsoleColor.DarkGreen),
            new Skin(ConsoleColor.Red, ConsoleColor.White, ConsoleColor.DarkGray, ConsoleColor.White),
            new Skin(ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.DarkMagenta, ConsoleColor.Green)
        };
        public static string[,] Get(bool Show = true, int Face = 0, int Suit = 0) {
            if (Show) {
                if (Face == 9) {
                    return new string[,] {
                        { "┌", "─", "─", "─", "─", "─", "┐"},
                        { "│", "1", "0", " ", " ", " ", "│"},
                        { "│", " ", " ", " ", " ", " ", "│"},
                        { "│", " ", " ", Suits.ElementAt(Suit).Key, " ", " ", "│"},
                        { "│", " ", " ", " ", " ", " ", "│"},
                        { "│", " ", " ", " ", "1", "0", "│"},
                        { "└", "─", "─", "─", "─", "─", "┘"}
                    };
                }
                else {
                    return new string[,] {
                        { "┌", "─", "─", "─", "─", "─", "┐"},
                        { "│", Names.Keys.ElementAt(Face), " ", " ", " ", " ", "│"},
                        { "│", " ", " ", " ", " ", " ", "│"},
                        { "│", " ", " ", Suits.ElementAt(Suit).Key, " ", " ", "│"},
                        { "│", " ", " ", " ", " ", " ", "│"},
                        { "│", " ", " ", " ", " ", Names.Keys.ElementAt(Face), "│"},
                        { "└", "─", "─", "─", "─", "─", "┘"}
                    };
                }
            }
            else {
                return new string[,] {
                    { "┌", "─", "─", "─", "─", "─", "┐"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "│", "?", " ", "?", " ", "?", "│"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "│", "?", " ", "?", " ", "?", "│"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "└", "─", "─", "─", "─", "─", "┘"}
                };
            }

        }
    }


    class Skin {
        Stopwatch sw;
        public readonly ConsoleColor fg1;
        public readonly ConsoleColor fg2;
        public readonly ConsoleColor bg;
        public readonly ConsoleColor back;
        public Skin(ConsoleColor _fg1, ConsoleColor _fg2, ConsoleColor _bg, ConsoleColor _back) {
            fg1 = _fg1;
            fg2 = _fg2;
            bg = _bg;
            back = _back;
            sw = new Stopwatch();
        }

        string[,] display = new string[,] {
                    { "┌", "─", "─", "─", "─", "─", "┐"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "│", "?", " ", "?", " ", "?", "│"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "│", "?", " ", "?", " ", "?", "│"},
                    { "│", " ", "?", " ", "?", " ", "│"},
                    { "└", "─", "─", "─", "─", "─", "┘"}
                };

        public void Draw(int left, int top) {
            sw.Restart();
            Console.BackgroundColor = bg;
            string[,] Visuals = CardVisuals.Get(true, 0, 0);
            Console.ForegroundColor = fg2;
            for (int y = 0; y < 2; y++) {
                Console.SetCursorPosition(left, top + y);
                for (int x = 0; x < Visuals.GetLength(1); x++) {
                    Console.Write(Visuals[y, x]);
                }
            }
            Visuals = CardVisuals.Get(true, 0, 1);
            Console.ForegroundColor = fg1;
            for (int y = 0; y < 2; y++) {
                Console.SetCursorPosition(left, top + 2 + y);
                for (int x = 0; x < Visuals.GetLength(1); x++) {
                    Console.Write(Visuals[y, x]);
                }
            }
            Visuals = CardVisuals.Get(false, 0, 0);
            Console.ForegroundColor = back;
            for (int y = 0; y < Visuals.GetLength(0); y++) {
                Console.SetCursorPosition(left, top + 4 + y);
                for (int x = 0; x < Visuals.GetLength(1); x++) {
                    Console.Write(Visuals[y, x]);
                }
            }
            Console.ResetColor();
        }
    }
}
