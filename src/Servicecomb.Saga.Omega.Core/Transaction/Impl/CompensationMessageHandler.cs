using System;
using System.Collections.Generic;
using System.Text;
using Servicecomb.Saga.Omega.Core.Context;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{
    public class CompensationMessageHandler: IMessageHandler
    {
        private readonly IMessageSender _sender;

        private readonly CompensationContext _compensationContext;

        public CompensationMessageHandler(IMessageSender sender, CompensationContext context)
        {
            _sender = sender;
            _compensationContext = context;
        }

        public void OnReceive(string globalTxId, string localTxId, string parentTxId, string compensationMethod,
            params object[] payloads)
        {
            _compensationContext.Apply(globalTxId, localTxId, compensationMethod, payloads);
            _sender.Send(new TxCompensatedEvent(globalTxId, localTxId, parentTxId, compensationMethod));
        }
    }
}
