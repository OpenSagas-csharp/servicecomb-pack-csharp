namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class TxCompensatedEvent:TxEvent
    {
        public TxCompensatedEvent(string globalTxId, string localTxId, string parentTxId, string compensationMethod) : base(EventType.TxCompensatedEvent, globalTxId, localTxId, parentTxId, compensationMethod, 0, "", 0)
        {
        }
    }
}
