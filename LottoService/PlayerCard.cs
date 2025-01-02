using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LottoService
{
    [DataContract]
    public class PlayerCard
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int RoundNumber { get; set; }
        [DataMember]
        public int GuessNumber1 { get; set; }
        [DataMember]
        public int GuessNumber2 { get; set; }
        [DataMember]
        public decimal Bet { get; set; }
        [DataMember]
        public decimal Score { get; set; }
        [DataMember]
        public int NumberOfMatches { get; set; }
        [DataMember]
        public int Rank { get; set; }
        [DataMember]
        public List<int> drawnNumbers { get; set; }
        [DataMember]
        public IPlayerCallback Callback { get; set; }
    }
}