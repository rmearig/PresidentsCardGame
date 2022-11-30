using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presidents.Game_Pieces
{
    public class GameManager
    {
        public List<Player> Players { get; set; }
        public List<List<Card>> DiscardPile { get; set; }
        public CardDeck Deck { get; set; }
        public int NumberOfDecks { get; set; }

        public GameManager(List<Player> players, int numberOfDecks)
        {
            Players = players;
            NumberOfDecks = numberOfDecks;
            DiscardPile = new List<List<Card>>();

            Deck = new CardDeck(NumberOfDecks);
            Deck.Shuffle();

            int maxCards = NumberOfDecks * 54;
            int dealtCards = 0;

            while (dealtCards < maxCards)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (dealtCards >= maxCards)
                    {
                        break;
                    }
                    Players[i].DealtCards.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                    dealtCards++;
                }
            }

        }
        
        public void PlayGame(int numOfDecks)
        {
            int round = 1;
            
            var playAgain = true;

            while (playAgain)
            {
                int i = 1;
                int w = 1;
                var currentTurn = new PlayerTurn() { Card = null };
                var futurePos = 1;

                if (round != 1)
                {
                    //change the cards around
                    GameManager manager = new GameManager(Players, numOfDecks);
                    foreach (var player in Players)
                    {
                        player.DealtCards.Sort((a, b) => a.Value.CompareTo(b.Value));
                    }
                    for (int p = 0; p < 2; p++)
                    {
                        Players.Last().DealtCards.Add(Players.First().DealtCards.First());
                        Players.First().DealtCards.Remove(Players.First().DealtCards.First());
                    }
                    for (int s = 0; s < 2; s++)
                    {
                        Players.Last().DealtCards.Sort((a, b) => a.Value.CompareTo(b.Value));
                        Players.First().DealtCards.Add(Players.Last().DealtCards.Last());
                        Players.Last().DealtCards.Remove(Players.Last().DealtCards.Last());
                    }
                    for (int vp = 0; vp < 1; vp++)
                    {
                        Players[Players.Count-2].DealtCards.Add(Players[1].DealtCards.First());
                        Players[1].DealtCards.Remove(Players[1].DealtCards.First());
                    }
                    for (int vs = 0; vs < 1; vs++)
                    {
                        Players[Players.Count - 2].DealtCards.Sort((a, b) => a.Value.CompareTo(b.Value));
                        Players[1].DealtCards.Add(Players[Players.Count - 2].DealtCards.Last());
                        Players[Players.Count - 2].DealtCards.Remove(Players[1].DealtCards.Last());
                    }
                }
                
                while (Players.Any(p => p.FuturePosition == 0))
                {
                    i = i > Players.Count ? 1 : i;
                    var currentPlayer = Players[i - 1];
                    if (currentPlayer.FuturePosition == 0 && currentPlayer.DealtCards.Count == 0)
                    {
                        currentPlayer.FuturePosition = futurePos;
                        futurePos++;
                    }

                    if (currentPlayer.FuturePosition == 0)
                    {
                        Console.WriteLine($"Alright, {currentPlayer.Name}, your turn!");
                        var previousCard = currentTurn.Card;
                        currentTurn = currentPlayer.Turn(currentPlayer.Hand, currentTurn.Card);

                        if (currentTurn.Card[0].Value == CardValue.Joker)
                        {
                            w = i;
                            foreach (var player in Players)
                            {
                                player.HasPlayed = true;
                            }
                        }

                        if (previousCard != currentTurn.Card)
                            w = i;

                        var winningPlayer = Players[w - 1];

                        AddToDiscardPile(currentTurn);
                        i++;
                    }
                    else
                    {
                        currentPlayer.HasPlayed = true;
                        i++;
                    }


                    if (Players.All(p => p.HasPlayed == true))
                    {
                        for (int p = 0; p < Players.Count; p++)
                        {
                            Players[p].HasPlayed = false;
                        }
                        i = w;
                        currentTurn.Card = null;
                    }
                }

                Players.Sort((a, b) => a.FuturePosition.CompareTo(b.FuturePosition));
                Console.WriteLine("That round is over. Here are the results:");
 
                foreach (var player in Players)
                {
                    string role;
                    switch (player.FuturePosition)
                    {
                        case 1:
                            role = "the President";
                            break;
                        case 2:
                            role = "the Vice President";
                            break;
                        default:
                            role = "a Citizen";
                            break;
                    }
                    if (player == Players[Players.Count-2])
                    {
                        role = "the Lower Scum";
                    }
                    else if (player == Players.Last())
                    {
                        role = "the Lowest Scum";
                    }
                    Console.WriteLine($"{player.Name} is {role}");
                }

                Console.WriteLine("Do you want to play again? y/n");
                string keepPlaying = Console.ReadLine();
                playAgain = keepPlaying == "y" ? true : false;
                if (playAgain)
                {
                    round++;
                    foreach (var player in Players)
                    {
                        player.CurrentPosition = player.FuturePosition;
                        player.FuturePosition = 0;
                        player.HasPlayed = false;
                    }
                    Players.Sort((a, b) => a.CurrentPosition.CompareTo(b.CurrentPosition));
                }
            }

            Console.WriteLine("Thanks for playing!");
            
            
        }

        private void AddToDiscardPile(PlayerTurn currentTurn)
        {
            DiscardPile.Insert(0, currentTurn.Card);
        }
    }
}
