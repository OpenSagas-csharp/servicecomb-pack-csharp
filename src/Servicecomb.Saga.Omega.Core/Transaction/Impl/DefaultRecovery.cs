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
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Transaction.Exception;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{


    /**
    * DefaultRecovery is used to execute business logic once.
    * The corresponding events will report to alpha server before and after the execution of business logic.
    * If there are errors while executing the business logic, a TxAbortedEvent will be reported to alpha.
    *
    *                 pre                       post
    *     request --------- 2.business logic --------- response
    *                 \                           /
    * 1.TxStartedEvent \                         / 3.TxEndedEvent
    *                   \                       /
    *                    ----------------------
    *                            alpha
    */
    public class DefaultRecovery : IRecoveryPolicy
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(DefaultRecovery));

        public void BeforeApply(IEventAwareInterceptor compensableInterceptor, OmegaContext omegaContext, string parentTxId, int retries, int timeout, string methodName, params object[] parameters)
        {
            _logger.Debug($"Intercepting compensable method {methodName} with context {omegaContext}");

            var response = compensableInterceptor.PreIntercept(parentTxId, methodName, timeout,
                "", retries, parameters);
            if (!response.Aborted) return;
            var abortedLocalTxId = omegaContext.GetLocalTxId();
            omegaContext.SetGlobalTxId(parentTxId);
            throw new InvalidTransactionException($"Abort sub transaction {abortedLocalTxId}  because global transaction{omegaContext.GetLocalTxId()} has already aborted.");
        }

        public void AfterApply(IEventAwareInterceptor compensableInterceptor, string parentTxId, string methodName)
        {
            compensableInterceptor.PostIntercept(parentTxId, methodName);
        }

        public void ErrorApply(IEventAwareInterceptor compensableInterceptor, string parentTxId, string methodName, System.Exception throwable)
        {
            compensableInterceptor.OnError(parentTxId, methodName, throwable);
        }
    }
}
