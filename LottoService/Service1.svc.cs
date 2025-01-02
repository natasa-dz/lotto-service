using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace LottoService
{
    public class LottoService : ILotto, IPlayer
    {

        public static List<int> Numbers=new List<int>();
        public static Dictionary<string,PlayerCard> Dictionary=new Dictionary<string,PlayerCard>();
        public static Timer TickTimer;


        public void LottoGame()
        {
            TickTimer = new Timer(roundStart, null, 0, 60000);

        }

        private void roundStart(object state)
        {
            int roundNumber = 1;
            Numbers = generateNumbers();
            foreach (var id in Dictionary)
            {
                List<int> userGuesses = new List<int>();
                userGuesses.Add(id.Value.GuessNumber1);
                userGuesses.Add(id.Value.GuessNumber2);
                int matches = findMatches(userGuesses, Numbers);
                decimal score = 0;

                if (matches == 0)
                {
                    score = 0;

                }
                else if(matches == 1) {
                    score = id.Value.Bet;
                }
                else
                {
                    score=id.Value.Bet*5;
                }

                id.Value.drawnNumbers=Numbers;
                id.Value.Score=score;
                id.Value.NumberOfMatches=matches;
                id.Value.RoundNumber=roundNumber;
            }

            foreach (var id2 in Dictionary)
            {
                id2.Value.Rank = Dictionary.Values.Count(p => p.Score > id2.Value.Score) + 1;
                id2.Value.Callback.onRoundEnd(id2.Value);
                
            }

            Dictionary=new Dictionary<string,PlayerCard>();
        }

        public static List<int> generateNumbers()
        {
            int minValue = 1;
            int maxValue = 10;
            List<int> randomNumbers = new List<int>();
            byte[] randomNumber = new byte[1];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                //izmenjeno da ne generise 7 brojeva, nego dva
                for (int i = 0; i < 2; i++)
                {
                    do
                    { 
                        rng.GetBytes(randomNumber);
                    } 
                    while (!IsFair(randomNumber[0], maxValue - minValue + 1));

                    randomNumbers.Add(Math.Abs((randomNumber[0] % (maxValue - minValue + 1)) + minValue));
                }
            }

            return randomNumbers;
        }

        private int findMatches(List<int> user, List<int> round)
        {
            int counter = 0;
            foreach (int temp in round)
            {
                if (user.Contains(temp))
                {
                    counter++;
                }

            }
            return counter;
        }


        private static bool IsFair(byte randomByte, int range)
        {
            int fullSetsOfValues = Byte.MaxValue / range;
            return randomByte < range * fullSetsOfValues;
        }

        public string initPlayer(string id, int amount, int guess1, int guess2)
        {

            if (Dictionary.ContainsKey(id))
            {
                return "You can't have two bets in one round!";
            }

            Dictionary[id] = new PlayerCard
            {
                Id = id,
                Bet = amount,
                GuessNumber1 = guess1,
                GuessNumber2 = guess2,
                Callback = OperationContext.Current.GetCallbackChannel<IPlayerCallback>()
            };

            return "User successfully registered: ID {"+ id+"}\n";
        }

        //TODO: IMPLEMENT
        public PlayerCard getPlayerCard(string id)
        {
            if (Dictionary.TryGetValue(id, out PlayerCard playerCard))
            {
                return playerCard;
            }
            // Handle the case where the player ID is not found
            throw new FaultException($"Player with ID {id} not found.");
        }
    }
}
