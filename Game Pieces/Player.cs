using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presidents.Game_Pieces
{
    public class Player
    {
        public string Name { get; set; }
        public int CurrentPosition { get; set; }
        public int FuturePosition { get; set; }
        public bool HasPlayed { get; set; }
        public List<Card> DealtCards { get; set; }
        public List<List<Card>> Hand
        {
            get
            {
                var hand = new List<List<Card>>();
                foreach (CardValue cardValue in Enum.GetValues(typeof(CardValue)))
                {
                    var cards = new List<Card>();
                    foreach (var card in DealtCards)
                    {
                        if (card.Value == cardValue)
                        {
                            cards.Add(card);
                        }
                    }
                    if (cards.Count > 0)
                        hand.Add(cards);
                }
                return hand;
            }
            set
            {

            }
        }

        public Player()
        {
            DealtCards = new List<Card>();
            FuturePosition = 0;
        }

        public PlayerTurn Turn(List<List<Card>> currentHand, List<Card> currentDiscard)
        {
            PlayerTurn turn = new PlayerTurn();
            string lastCardMessage = currentDiscard == null ? "You start it off.\n" : $"This is the last card(s) played: \n{currentDiscard.Count} - {currentDiscard[0].Value}\n";
            Console.WriteLine(lastCardMessage);
            
            Console.WriteLine("And this is your hand: (press Enter to view hand)");
            Console.ReadKey();
            foreach (var cardList in currentHand)
            {
                string plural = cardList.Count > 1 ? "s" : string.Empty;
                Console.WriteLine($"{cardList.Count} - {cardList[0].Value}{plural}");
            }

            if (currentDiscard == null)
            {
                turn = Play(currentHand, currentDiscard);
            }
            else
            {
                Console.WriteLine("Would you like to (1) Play or (2) Skip?");
                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);
                while (!validChoice || (choice < 1 || choice > 2))
                {
                    Console.WriteLine("Please enter a valid option.");
                    validChoice = int.TryParse(Console.ReadLine(), out choice);
                }
                turn.Result = choice == 1 ? TurnResult.Play : TurnResult.Skip;
                if (turn.Result == TurnResult.Play && HasPlayableCard(currentHand, currentDiscard)) { turn = Play(currentHand, currentDiscard); }
                else
                {
                    if (!HasPlayableCard(currentHand, currentDiscard) && choice == 1)
                        Console.WriteLine("You don't have a playable card, you must skip.");
                    turn = Skip(currentDiscard);
                }
            }
            
            HasPlayed = true;

            return turn;
        }

        private bool HasPlayableCard(List<List<Card>> currentHand, List<Card> currentDiscard)
        {
            bool playableCard = false;
            if (currentDiscard == null)
                return true;

            foreach (var card in currentHand)
            {
                if (card.Count >= currentDiscard.Count && card[0].Value > currentDiscard[0].Value || card[0].Value == CardValue.Joker)
                {
                    playableCard = true;
                    break;
                }
            }
            return playableCard;
        }

        private PlayerTurn Play(List<List<Card>> currentHand, List<Card> currentDiscard)
        {
            PlayerTurn turn = new PlayerTurn();

            turn.Result = TurnResult.Play;
            turn.Card = currentDiscard;
            var playableOptions = new List<List<Card>>();
            if (currentDiscard == null)
            {
                playableOptions = currentHand;
            }
            else
            {
                foreach (var cardList in currentHand)
                {
                    if (cardList.Count >= currentDiscard.Count && cardList[0].Value > currentDiscard[0].Value || cardList[0].Value == CardValue.Joker)
                    {
                        playableOptions.Add(cardList);
                    }
                }
            }

            Console.WriteLine("Which option would you like to play? Please select the corresponding number from below.");
            for (int p = 1; p <= playableOptions.Count; p++)
            {
                bool beginningOfRound = currentDiscard == null;
                string cardCount = string.Empty;
                if (!beginningOfRound)
                {
                    cardCount = playableOptions[p - 1].Count > currentDiscard.Count ? $"{currentDiscard.Count} of {playableOptions[p - 1].Count}" : $"{playableOptions[p - 1].Count}";
                }
                else
                {
                    cardCount = $"{playableOptions[p - 1].Count}";
                }
                Console.WriteLine($"{p}. {cardCount} - {playableOptions[p - 1][0].Value}");
            }

            int choice;
            bool validChoice = int.TryParse(Console.ReadLine(), out choice);
            while (!validChoice || (choice < 1 || choice > playableOptions.Count))
            {
                Console.WriteLine("Please enter a valid option.");
                validChoice = int.TryParse(Console.ReadLine(), out choice);
            }

            var numOfCards = currentDiscard != null ? currentDiscard.Count : playableOptions[choice - 1].Count;
            int cardLoc = 0;
            foreach (var cardList in Hand)
            {
                if (cardList[0].Value == playableOptions[choice - 1][0].Value)
                {
                    break;
                }
                cardLoc++;
            }
            numOfCards = Hand[cardLoc][0].Value == CardValue.Joker ? 1 : numOfCards;

            for (int c = 0; c < numOfCards; c++)
            {
                DealtCards.Remove(Hand[cardLoc][0]);
            }
            

            //var handLoc = Hand.Where(c => c.First().Value == playableOptions[choice - 1][0].Value).First();
            //Hand.Remove(handLoc);
            turn.Card = playableOptions[choice - 1].GetRange(0, numOfCards);

            Console.Clear();

            return turn;
        }

        private PlayerTurn Skip(List<Card> previousDiscard)
        {
            PlayerTurn turn = new PlayerTurn();
            turn.Card = previousDiscard;
            Console.Clear();
            Console.WriteLine("You chose to skip, next persons turn.");

            return turn;
        }
    }
}
