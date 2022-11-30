using System;
using System.Collections.Generic;
using System.Text;

namespace Presidents.Game_Pieces
{
    public class CardDeck
    {
        public List<Card> Cards { get; set; }
        public int NumberOfDecks { get; set; }

        public CardDeck(int numberOfDecks)
        {
            Cards = new List<Card>();
            NumberOfDecks = numberOfDecks;

            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                if (suit != CardSuit.Joker)
                {
                    foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                    {
                        if (value != CardValue.Joker)
                        {
                            for (int d = 1; d <= NumberOfDecks; d++)
                            {
                                Cards.Add(new Card()
                                {
                                    Suit = suit,
                                    Value = value
                                });
                            }                            
                        }
                    }
                }
                else
                {
                    for (int d = 1; d <= NumberOfDecks; d++)
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            Cards.Add(new Card()
                            {
                                Suit = CardSuit.Joker,
                                Value = CardValue.Joker
                            });
                        }
                    }
                }
            }
        }

        public void Shuffle()
        {
            Random r = new Random();

            List<Card> cards = Cards;

            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
        }
    }
}
