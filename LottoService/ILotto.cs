﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LottoService
{
    [ServiceContract]
    internal interface ILotto
    {
        [OperationContract]
        void LottoGame();
    }
}
