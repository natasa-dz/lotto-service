using System;
using System.Collections.Generic;
using System.ServiceModel;
using LottoServiceClient.ServiceReference1;

namespace LottoServiceClient
{
    public class CallBack : IPlayerCallback
    {
        public void onRoundEnd(PlayerCard playerCard)
        {
            Console.WriteLine();
            Console.WriteLine("************************* ROUND ENDED *************************");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"| Player ID:           | {playerCard.Id}");
            Console.WriteLine($"|------------------------------------------------------------|");
            Console.WriteLine($"| Drawn Numbers:       | {string.Join(", ", playerCard.drawnNumbers)}");
            Console.WriteLine($"|------------------------------------------------------------|");
            Console.WriteLine($"| Guess Numbers:       | {playerCard.GuessNumber1}, {playerCard.GuessNumber2}");
            Console.WriteLine($"|------------------------------------------------------------|");
            Console.WriteLine($"| Number of Matches:   | {playerCard.NumberOfMatches}");
            Console.WriteLine($"|------------------------------------------------------------|");
            Console.WriteLine($"| Score:               | {playerCard.Score}");
            Console.WriteLine($"|------------------------------------------------------------|");
            Console.WriteLine($"| Rank:                | {playerCard.Rank}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            Program.RoundEnded = true;
        }
    }

    internal class Program
    {
        private static PlayerClient _playerClient;
        private static HashSet<string> _registeredPlayers = new HashSet<string>();
        public static bool RoundEnded = false;

        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new CallBack());
            _playerClient = new PlayerClient(ic);
            LottoGameStart();
        }

        public static void LottoGameStart()
        {
            while (true)
            {
                Console.WriteLine("Enter Player ID (or type 'exit' to quit):");
                string playerId = Console.ReadLine();
                if (playerId.ToLower() == "exit") break;

                if (_registeredPlayers.Contains(playerId))
                {
                    Console.WriteLine("You can't have two bets in one round!\n");
                    continue; // Allow the user to enter a new ID
                }

                int betAmount = GetValidInput("Enter Bet Amount (positive integer):", value => int.TryParse(value, out int result) && result > 0);
                int guess1 = GetValidInput("Enter First Guess (1-10):", value => int.TryParse(value, out int result) && result >= 1 && result <= 10);
                int guess2 = GetValidInput("Enter Second Guess (1-10):", value => int.TryParse(value, out int result) && result >= 1 && result <= 10);

                try
                {
                    string result = _playerClient.initPlayer(playerId, betAmount, guess1, guess2);
                    Console.WriteLine(result);

                    if (result.Contains("successfully"))
                    {
                        _registeredPlayers.Add(playerId);
                        Console.WriteLine("Waiting for the round to end...\n");
                        WaitForRoundToEnd();
                        _registeredPlayers.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        private static int GetValidInput(string prompt, Func<string, bool> validate)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                if (input.ToLower() == "exit") Environment.Exit(0); // Allow exit at any prompt
            } while (!validate(input));

            return int.Parse(input);
        }

        private static void WaitForRoundToEnd()
        {
            RoundEnded = false;

            while (!RoundEnded)
            {
                // Allow user to type "exit" to quit while waiting
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    return;
                }

                System.Threading.Thread.Sleep(100); // Small delay to avoid busy-waiting

                if (_playerClient.State == CommunicationState.Faulted || _playerClient.State == CommunicationState.Closed)
                {
                    _playerClient = new PlayerClient(new InstanceContext(new CallBack()));
                }
            }
        }
    }
}
