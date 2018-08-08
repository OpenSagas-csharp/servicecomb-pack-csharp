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

using System;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Logging;

namespace Servicecomb.Saga.Omega.Core.Transaction.Impl
{


  /**
 * DefaultRecovery is used to execute business logic once.
 * The corresponding events will report to alpha server before and after the execution of business logic.
 * If there are errors while executing the business logic, a TxAbortedEvent will be reported to alpha.
 *
 *                 pre                       post
 *     request --------- 2.business logic --------- response
 *                 \                          |
 * 1.TxStartedEvent \                        | 3.TxEndedEvent
 *                   \                      |
 *                    ----------------------
 *                            alpha
 */
  public class DefaultRecovery:IRecoveryPolicy
  {
    private readonly ILogger _logger= LogManager.GetLogger(typeof(DefaultRecovery));

    public object Apply(CompensableInterceptor compensableInterceptor, OmegaContext context, string parentTxId, int retries,
      CompensableAttribute compensable, string compentationMethod)
    {

      _logger.Debug($"Intercepting compensable method {compentationMethod} with context {context}");

      var response = compensableInterceptor.PreIntercept(parentTxId, compentationMethod, compensable.Timeout,
        compentationMethod, retries, new Object []{});
      if (response.Aborted)
      {
        String abortedLocalTxId = context.localTxId();
        context.setLocalTxId(parentTxId);
        throw new InvalidTransactionException("Abort sub transaction " + abortedLocalTxId +
                                              " because global transaction " + context.globalTxId() + " has already aborted.");
      }

      try
      {
        Object result = joinPoint.proceed();
        interceptor.postIntercept(parentTxId, compensationSignature);

        return result;
      }
      catch (Throwable throwable)
      {
        interceptor.onError(parentTxId, compensationSignature, throwable);
        throw throwable;
      }
      throw new NotImplementedException();
    }
  }
}
