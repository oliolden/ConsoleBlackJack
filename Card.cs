using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack {
    class Card {


        public int Suit; // 0 = Clubs, 1 = Diamonds, 2 = Hearts, 3 = Spades.
        public int Face; // 0 - 12.
        public bool Show; // True = Face visible, False = Back visible.
        public int Value;
        string[,] Visuals;
        public Card(int suit, int face, bool show) {
            Suit = suit;
            Face = face;
            Show = show;
            if (Face < 10)
                Value = Face + 1;
            else
                Value = 10;
            UpdateVisuals();
        }

        public void UpdateVisuals() {
            Visuals = CardVisuals.Get(Show, Face, Suit);
        }

        public override bool Equals(Object obj) {
            return (((Card)obj).Face == Face) && (((Card)obj).Suit == Suit) && obj is Card;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public void Draw(int left, int top, int skin) {
            Console.BackgroundColor = CardVisuals.Skins[skin].bg;
            if (!Show)
                Console.ForegroundColor = CardVisuals.Skins[skin].back;
            else if (Suit == 1 || Suit == 2)
                Console.ForegroundColor = CardVisuals.Skins[skin].fg1;
            else
                Console.ForegroundColor = CardVisuals.Skins[skin].fg2;
            for (int y = 0; y < Visuals.GetLength(0); y++) {
                Console.SetCursorPosition(left, top + y);
                for (int x = 0; x < Visuals.GetLength(1); x++) {
                    Console.Write(Visuals[y, x]);
                }
            }
            Console.ResetColor();
        }
    }
}
