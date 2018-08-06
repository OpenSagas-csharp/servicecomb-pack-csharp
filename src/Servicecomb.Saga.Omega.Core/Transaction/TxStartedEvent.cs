using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class TxStartedEvent : TxEvent
    {
        public TxStartedEvent(string globalTxId, string localTxId, string parentTxId, string compensationMethod, int timeout, string retryMethod, int retries, params object[] payloads) : base(EventType.TxStartedEvent, globalTxId, localTxId, parentTxId, compensationMethod, timeout, retryMethod, retries, payloads)
        {
        }
    }
}
