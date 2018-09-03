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

using System.Transactions;
using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Transaction.Exception;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{
    public class SagaStartAnnotationProcessor : ISagaStartEventAwareInterceptor
    {

        private readonly OmegaContext _omegaContext;
        private readonly IMessageSender _sender;

        public SagaStartAnnotationProcessor(OmegaContext omegaContext, IMessageSender sender)
        {
            _omegaContext = omegaContext;
            _sender = sender;
        }
        public AlphaResponse PreIntercept(string parentTxId, string compensationMethod, int timeout, string retriesMethod, int retries,
          params object[] message)
        {
            try
            {
                return _sender.Send(new SagaStartedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetLocalTxId(), timeout));
            }
            catch (OmegaException ex)
            {
                throw new TransactionException(ex.Message, ex);
            }
        }

        public void PostIntercept(string parentTxId, string compensationMethod)
        {
            var response = _sender.Send(new SagaEndedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetLocalTxId()));
            if (response.Aborted)
            {
                throw new OmegaException($"transaction {parentTxId}  is aborted");
            }
        }

        public void OnError(string parentTxId, string compensationMethod, System.Exception throwable)
        {
            _sender.Send(new TxAbortedEvent(_omegaContext.GetGlobalTxId(), _omegaContext.GetGlobalTxId(), null, compensationMethod, throwable));
        }
    }
}
