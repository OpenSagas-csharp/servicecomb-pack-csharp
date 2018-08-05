using Servicecomb.Saga.Omega.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{
    public class CompensableInterceptor : IEventAwareInterceptor
    {
        private OmegaContext omegaContext;
        private IMessageSender _sender;

        CompensableInterceptor(OmegaContext context, IMessageSender sender)
        {
            _sender = sender;
            omegaContext = context;
        }
        public void OnError(string parentTxId, string compensationMethod, Exception throwable)
        {
        //    return _sender.Send(new TxStartedEvent(omegaContext.GlobalTxId(), context.localTxId(), parentTxId, compensationMethod,
        //timeout, retriesMethod, retries, message));
        }

        public void PostIntercept(string parentTxId, string compensationMethod)
        {
            throw new NotImplementedException();
        }

        public AlphaResponse PreIntercept(string parentTxId, string compensationMethod, int timeout, string retriesMethod, int retries, params object[] message)
        {
            throw new NotImplementedException();
        }
    }
}
