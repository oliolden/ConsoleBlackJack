using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack {
    class Dealer : Player {
        public Dealer() {
        }
        protected override void Init() {
            name = "Dealer";
            Hand = new List<Card>();
            ShowValue = false;
            l1 = 7;
            l2 = 9;
            // Dealer starter cards.
            AddCard(false);
            AddCard(true);
            UpdateHandValue();
        }
    }
}
