using Servicecomb.Saga.Omega.Core.Transaction.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class TxEvent
    {
        public long Timestamp { get; set; }
        public EventType Type { get; set; }
        public string GlobalTxId { get; set; }
        public string LocalTxId { get; set; }
        public string ParentTxId { get; set; }
        public string CompensationMethod { get; set; }
        public int Timeout { get; set; }
        public Object[] Payloads { get; set; }

        public string RetryMethod { get; set; }
        public int Retries { get; set; }

        public TxEvent(EventType type, String globalTxId, String localTxId, String parentTxId, String compensationMethod,
            int timeout, String retryMethod, int retries, params Object[] payloads)
        {
            this.Timestamp = DateTimeExtensions.CurrentTimeMillis();
            this.Type = type;
            this.GlobalTxId = globalTxId;
            this.LocalTxId = localTxId;
            this.ParentTxId = parentTxId;
            this.CompensationMethod = compensationMethod;
            this.Timeout = timeout;
            this.RetryMethod = retryMethod;
            this.Retries = retries;
            this.Payloads = payloads;
        }


        public override string ToString()
        {
            return Type.ToString() + "{" +
                "globalTxId='" + GlobalTxId + '\'' +
                ", localTxId='" + LocalTxId + '\'' +
                ", parentTxId='" + ParentTxId + '\'' +
                ", compensationMethod='" + CompensationMethod + '\'' +
                ", timeout=" + Timeout +
                ", retryMethod='" + RetryMethod + '\'' +
                ", retries=" + Retries +
                ", payloads=" + string.Join(",",
                          Payloads.Select(x => x.ToString()).ToArray()) +
                '}';
        }
    }
}
