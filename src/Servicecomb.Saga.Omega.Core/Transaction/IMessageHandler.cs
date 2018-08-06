using System;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public interface IMessageHandler
    {
        void OnReceive(string globalTxId, string localTxId, string parentTxId, string compensationMethod, params Object[] payloads);
    }
}
