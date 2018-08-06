using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class TxAbortedEvent: TxEvent
    {
        private const int PayloadsMaxLength = 10240;

        public TxAbortedEvent(string globalTxId, string localTxId, string parentTxId, string compensationMethod, System.Exception throwable) : base(EventType.TxAbortedEvent, globalTxId, localTxId, parentTxId, compensationMethod, 0, "", 0,
            StackTrace(throwable))
        {
        }

        private static string StackTrace(System.Exception throwable)
        {
            if (throwable.StackTrace.Length > PayloadsMaxLength)
            {
                return throwable.StackTrace.Substring(PayloadsMaxLength);
            }

            return throwable.StackTrace;
        }
    }
}
