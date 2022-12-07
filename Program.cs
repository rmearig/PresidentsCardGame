using Presidents.Game_Pieces;
using System;
using System.Collections.Generic;

namespace Presidents
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(  "* * * * * * * ============= \n" +
                                " * * * * * *  ============= \n" +
                                "* * * * * * * ============= \n" +
                                " * * * * * *  ============= \n" +
                                "=========================== \n" +
                                "=========================== \n" +
                                "===========================\n");


            Console.WriteLine("Ready to play Presidents? Press enter to begin.");
            Console.ReadKey();

            Console.WriteLine("Perfect! How many players are there? Enter a number between 4 and 10");
            int numOfPlayers;
            bool validPlayerCount = int.TryParse(Console.ReadLine(), out numOfPlayers);
            while (!validPlayerCount || (numOfPlayers < 4 || numOfPlayers > 10))
            {
                Console.WriteLine("Please enter a valid number.");
                validPlayerCount = int.TryParse(Console.ReadLine(), out numOfPlayers);
            }

            var players = new List<Player>();

            for (int p = 1; p <= numOfPlayers; p++)
            {
                Console.WriteLine($"What is player {p}'s name:");
                var player = new Player()
                {
                    Name = Console.ReadLine(),
                    CurrentPosition = p
                };
                players.Add(player);
            }

            //foreach (var player in players)
            //{
            //    Console.WriteLine(player.Name);
            //}

            Console.WriteLine("How many decks would you like to play with? (1 or 2)");
            int numOfDecks;
            bool validDeckCount = int.TryParse(Console.ReadLine(), out numOfDecks);
            while (!validDeckCount || (numOfDecks < 1 || numOfDecks > 2))
            {
                Console.WriteLine("Please enter a valid number.");
                validDeckCount = int.TryParse(Console.ReadLine(), out numOfDecks);
            }

            Console.WriteLine("Perfect, let's start the game!");

            GameManager manager = new GameManager(players, numOfDecks);
            manager.PlayGame(numOfDecks);

            Console.ReadKey();

        }
    }
}
