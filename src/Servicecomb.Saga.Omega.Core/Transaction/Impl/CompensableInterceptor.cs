/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Transaction;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{
    public class CompensableInterceptor : IEventAwareInterceptor
    {
        private readonly OmegaContext _omegaContext;
        private readonly IMessageSender _sender;

        public CompensableInterceptor(OmegaContext context, IMessageSender sender)
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
