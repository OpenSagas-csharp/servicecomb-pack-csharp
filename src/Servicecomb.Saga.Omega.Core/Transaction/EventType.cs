using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public enum EventType
    {
        SagaStartedEvent,
        TxStartedEvent,
        TxEndedEvent,
        TxAbortedEvent,
        TxCompensatedEvent,
        SagaEndedEvent
    }
}
