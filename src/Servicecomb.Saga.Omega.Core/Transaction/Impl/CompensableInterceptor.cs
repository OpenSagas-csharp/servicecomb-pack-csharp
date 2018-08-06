using Servicecomb.Saga.Omega.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{
    public class CompensableInterceptor : IEventAwareInterceptor
    {
        private readonly OmegaContext _omegaContext;
        private readonly IMessageSender _sender;

        CompensableInterceptor(OmegaContext context, IMessageSender sender)
        {
            _sender = sender;
            _omegaContext = context;
        }
        public void OnError(string parentTxId, string compensationMethod, System.Exception throwable)
        {
            _sender.Send(new TxAbortedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetLocalTxId(), parentTxId, compensationMethod,
                throwable));
        }

        public void PostIntercept(string parentTxId, string compensationMethod)
        {
             _sender.Send(new TxEndedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetLocalTxId(), parentTxId, compensationMethod));
        }

        public AlphaResponse PreIntercept(string parentTxId, string compensationMethod, int timeout, string retriesMethod, int retries, params object[] message)
        {
            return _sender.Send(new TxStartedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetLocalTxId(), parentTxId, compensationMethod,
                timeout, retriesMethod, retries, message));
        }
    }
}
