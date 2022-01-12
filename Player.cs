using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack {
    class Player {
        public List<Card> Hand;
        public int HandValue;
        public bool ShowValue;
        protected int l1, l2; // Output lines;
        protected string name;
        public Player() {
            Init();
        }

        protected virtual void Init() {
            name = "You";
            Hand = new List<Card>();
            ShowValue = true;
            l1 = 18;
            l2 = 20;
            // Add starter cards.
            AddCard(true);
            AddCard(true);
            UpdateHandValue();
            Game.OutputDict[l2] = $"You drew a {CardVisuals.Names.ElementAt(Hand[0].Face).Value} of {CardVisuals.Suits.ElementAt(Hand[0].Suit).Value} and a {CardVisuals.Names.ElementAt(Hand[1].Face).Value} of {CardVisuals.Suits.ElementAt(Hand[1].Suit).Value}.";

        }

        public void UpdateHandValue() {
            CalcHand();
            while (HandValue > 21 && Hand.Any(c => c.Value == 11)) {
                Hand.Find(c => c.Value == 11).Value = 1;
                CalcHand();
            }
            if (ShowValue) {
                Game.OutputDict[l1] = $"Total: {HandValue}";
            }
        }
        private void CalcHand() {
            HandValue = 0;
            foreach (Card card in Hand) {
                HandValue += card.Value;
            }
        }

        public void AddCard(bool show) {
            Card card = Game.RandomCard(show);
            if (ShowValue)
                Game.OutputDict[l2] = $"{name} drew a {CardVisuals.Names.ElementAt(card.Face).Value} of {CardVisuals.Suits.ElementAt(card.Suit).Value}.";
            if (card.Face == 0) {
                if (HandValue + 11 > 21)
                    card.Value = 1;
                else
                    card.Value = 11;
            }
            Hand.Add(card);
            UpdateHandValue();
        }
    }
}
