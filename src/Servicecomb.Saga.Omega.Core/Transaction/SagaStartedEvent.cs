using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    class SagaStartedEvent: TxEvent
    {
        public SagaStartedEvent(string globalTxId, string localTxId,int timeout) : base(EventType.SagaStartedEvent, globalTxId, localTxId, null, "", timeout, "", 0)
        {
        }
    }
}
