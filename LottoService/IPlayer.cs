using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LottoService
{
    [ServiceContract(CallbackContract = typeof(IPlayerCallback))]
    internal interface IPlayer

    {
        [OperationContract]
        string initPlayer(string id, int amount, int guess1, int guess2);

        [OperationContract]
        PlayerCard getPlayerCard(string id);


    }

    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void onRoundEnd(PlayerCard playerCard);
    }
}
