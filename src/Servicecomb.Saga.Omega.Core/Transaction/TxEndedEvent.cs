namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class TxEndedEvent: TxEvent
    {
        public TxEndedEvent( string globalTxId, string localTxId, string parentTxId, string compensationMethod) : base(EventType.TxEndedEvent, globalTxId, localTxId, parentTxId, compensationMethod, 0, "", 0)
        {
        }
    }
}
