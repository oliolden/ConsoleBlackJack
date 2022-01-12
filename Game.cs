using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Black_Jack {
    class Game : Scene {
        public static Dictionary<int, string> OutputDict;
        public Player Player1;
        public Dealer CurrentDealer;
        public static Random rand = new Random();
        public bool gameLoop;
        bool stay = false;
        public static Dictionary<Card, int> ActiveCards { get; set; }
        bool playerBlackjack = false;
        bool playerBust = false;
        int top = (Console.WindowHeight / 2) - 10;
        private long Bet;
        private bool clearDealer, clearPlayer;

        public void RenderOutputs() {
            string blank;
            try {
                foreach (var item in OutputDict) {
                    blank = new string(' ', (Console.WindowWidth-item.Value.Length-2)/2);
                    Console.SetCursorPosition((Console.WindowWidth / 2) - ((item.Value.Length / 2)+blank.Length), item.Key + top);
                    Console.Write(blank+item.Value+blank);
                }
            }
            catch (InvalidOperationException) { }
        }

        public static Card RandomCard(bool show) // Returns a random card.
        {
            if (ActiveCards.Count() >= 52) {
                bool full = true;
                foreach (int count in ActiveCards.Values) {
                    if (count < 2) {
                        full = false;
                    }
                }
                if (full)
                    throw new Exception("All cards are drawn");
            }
            Card card;
            while (true) {
                card = new Card(rand.Next(4), rand.Next(13), true);

                if (ActiveCards.Count == 0)
                    ActiveCards.Add(card, 1);
                else {
                    bool isActive = false;
                    foreach (Card item in ActiveCards.Keys) // Check if the new card is active in the game.
                    {
                        if (item.Equals(card)) {
                            isActive = true;
                            card = item;
                        }
                    }
                    if (isActive) {
                        if (ActiveCards[card] > 1) // Card has been drawn twice and cannot be used.
                        {
                            continue;
                        }
                        else // Card has been drawn once.
                        {
                            ActiveCards[card]++;
                            break;
                        }
                    }
                    else // Card has not been drawn before.
                    {
                        ActiveCards.Add(card, 1);
                        break;
                    }
                }
            }
            return new Card(card.Suit, card.Face, show);
        }



        public void RenderBet() {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($"Bet: {Bet}");
        }

        public void Start() {
            Console.ResetColor();
            OutputDict = new Dictionary<int, string>();

            // Set up objects.
            ActiveCards = new Dictionary<Card, int>();
            Player1 = new Player();
            CurrentDealer = new Dealer();

            // Set the betting chips.
            Betting Betting = new Betting();
            Bet = Betting.TakeBet();
            if (Bet == -1) { return; }
            Program.graphics.CurrentScene = this;

            // Initiate main game loop.
            GameLoop();
        }
        public override void Render() {
            while (true) {
                try {
                    top = (Console.WindowHeight / 2) - 10;
                    Console.CursorVisible = false;
                    // Draw Dealer Card
                    if (clearDealer) {
                        for (int i = 0; i < 7; i++) {
                            Program.ClearLine(i + top);
                        }
                        clearDealer = false;
                    }
                    for (int i = 0; i < CurrentDealer.Hand.Count(); i++) {
                        CurrentDealer.Hand[i].Draw((Console.WindowWidth / 2) - (((11 * CurrentDealer.Hand.Count()) - 3) / 2) + (11 * i), top, Program.EquippedSkin);
                    }

                    // Draw Player Cards
                    if (clearPlayer) {
                        for (int i = 0; i < 7; i++) {
                            Program.ClearLine(i + 11 + top);
                        }
                        clearPlayer = false;
                    }
                    for (int i = 0; i < Player1.Hand.Count(); i++) {
                        Player1.Hand[i].Draw((Console.WindowWidth / 2) - (((11 * Player1.Hand.Count()) - 3) / 2) + (11 * i), 11 + top, Program.EquippedSkin);
                    }

                    RenderBet();
                    RenderOutputs();
                    break;
                }
                catch (Exception) {
                    Console.Clear();
                    string text = "Window too small.";
                    Console.SetCursorPosition((Console.WindowWidth / 2) - (text.Length / 2), (Console.WindowHeight / 2));
                    Console.Write(text);
                    continue;
                }
            }
        }

        void HandleInput() {
            ConsoleKey key;
            do {
                key = Console.ReadKey(true).Key;
            } while (Console.KeyAvailable);

            switch (key) {
                case ConsoleKey.Spacebar:
                    Player1.AddCard(true);
                    clearPlayer = true;
                    break;
                case ConsoleKey.Enter:
                    stay = true;
                    break;
                default:
                    break;
            }
        }
        public void PlayDealer() {
            CurrentDealer.Hand[0].Show = true;
            CurrentDealer.Hand[0].UpdateVisuals();
            CurrentDealer.ShowValue = true;
            CurrentDealer.UpdateHandValue();
            Game.OutputDict[9] = $"Dealer drew a {CardVisuals.Names.ElementAt(CurrentDealer.Hand[0].Face).Value} of {CardVisuals.Suits.ElementAt(CurrentDealer.Hand[0].Suit).Value} and a {CardVisuals.Names.ElementAt(CurrentDealer.Hand[1].Face).Value} of {CardVisuals.Suits.ElementAt(CurrentDealer.Hand[1].Suit).Value}.";

            while (CurrentDealer.HandValue < 17) {
                Thread.Sleep(1500);
                CurrentDealer.AddCard(true);
                clearDealer = true;
            }
        }
        void GameUpdate() {
            bool win = false;
            bool push = false;

            if (Player1.HandValue == 21) {
                //Player Blackjack
                playerBlackjack = true;
                win = true;
                gameLoop = false;
                OutputDict[20] = "Blackjack!";
                stay = true;
            }
            else if (Player1.HandValue > 21) {
                //Player Bust
                playerBust = true;
                gameLoop = false;
                OutputDict[20] = "Bust!";
                stay = true;
            }
            if (stay) {
                gameLoop = false;
                PlayDealer();
                if (CurrentDealer.HandValue == 21) {
                    //Dealer Blackjack
                    OutputDict[9] = "Blackjack!";
                    if (playerBlackjack) {
                        push = true;
                        win = false;
                    }
                }
                else if (CurrentDealer.HandValue > 21) {
                    //Dealer Bust
                    OutputDict[9] = "Bust!";
                    if (!playerBust)
                        win = true;
                }
                else if (!playerBlackjack && !playerBust) {
                    if (Player1.HandValue > CurrentDealer.HandValue)
                        win = true;
                    else if (Player1.HandValue == CurrentDealer.HandValue)
                        push = true;
                }
            }
            if (win) {
                // Win
                OutputDict[20] = "You win!";
                Program.chips += Bet;
                Program.Save();
            }
            else if (push) {
                // Push
                OutputDict[20] = "It's a push!";
            }
            else if (!gameLoop) {
                // Lose
                OutputDict[20] = "Dealer wins!";
                Program.chips -= Bet;
                Program.Save();
            }
        }

        void GameLoop() {
            gameLoop = true;

            GameUpdate();

            while (gameLoop) {
                // Handle Inputs without pausing.
                if (Console.KeyAvailable) {
                    HandleInput();
                }

                // Process Game Events
                GameUpdate();
            }
            while (Console.KeyAvailable) Console.ReadKey();
            Console.ReadKey(true);
        }
    }
    class Betting : Scene {
        static string input;
        static string row1;
        static string row2;
        static string row3;

        public override void Render() {
            Console.CursorVisible = false;
            Console.SetCursorPosition((Console.WindowWidth / 2) - (row1.Length / 2), Console.WindowHeight / 2 - 2);
            Console.Write(row1);
            Console.SetCursorPosition((Console.WindowWidth / 2) - (row2.Length / 2), Console.WindowHeight / 2);
            Console.Write(row2);
            Console.SetCursorPosition((Console.WindowWidth / 2) - (row3.Length / 2), Console.WindowHeight / 2 + 2);
            Console.Write(row3);
            Program.RenderChips();
        }
        public long TakeBet() {
            Program.graphics.CurrentScene = this;
            input = "";
            row1 = "How much do you want to bet?";
            row2 = "";
            row3 = "";
            long Bet;

            while (true) {
                try {
                    while (Console.KeyAvailable) {
                        char inChar = Console.ReadKey(true).KeyChar;
                        if (inChar == '\u001b')
                            return -1;
                        if (inChar == '\r') {
                            try {
                                if (Convert.ToInt64(input) > 0) {
                                    if (Convert.ToInt64(input) <= Program.chips) {
                                        Bet = Convert.ToInt64(input);
                                        //Program.chips -= Bet;
                                        //Program.Save();
                                        Graphics.clear = true;
                                        return Bet;
                                    }
                                    else {
                                        row3 = "Insufficient funds.";
                                        Graphics.clear = true;
                                    }
                                }
                                else {
                                    row3 = "Bet must be bigger than 0.";
                                    Graphics.clear = true;
                                }
                            }
                            catch (FormatException) {
                                row3 = "Bet must be an integer.";
                                Graphics.clear = true;
                            }
                        }
                        else if (inChar == '\b') {
                            input = input.Substring(0, input.Length - 1);
                            Graphics.clear = true;
                        }
                        else
                            input += inChar;
                        row2 = input;
                        Graphics.clear = true;
                    }
                }
                catch (ArgumentOutOfRangeException) {

                    continue;
                }
            }
        }
    }
}
