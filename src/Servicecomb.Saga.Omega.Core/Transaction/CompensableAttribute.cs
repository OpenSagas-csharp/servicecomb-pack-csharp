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
using MethodBoundaryAspect.Fody.Attributes;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CompensableAttribute : OnMethodBoundaryAspect
    {
        public int Retries { get; set; }

        public string CompensationMethod { get; set; }

        public int RetryDelayInMilliseconds { get; set; }

        public int Timeout { get; set; }

        private readonly ILogger _logger = LogManager.GetLogger(typeof(SagaStartAttributeAndAspect));

        private readonly CompensableInterceptor _compensableInterceptor;

        private readonly OmegaContext _omegaContext;

        private readonly IRecoveryPolicy _recoveryPolicy;

        public CompensableAttribute(IMessageSender sender, OmegaContext context, IRecoveryPolicy recoveryPolicy)
        {
            _omegaContext = context;
            _compensableInterceptor = new CompensableInterceptor(context, sender);
            _recoveryPolicy = recoveryPolicy;

        }

        public CompensableAttribute(string compensationMethod , int retryDelayInMilliseconds = 0, int timeout = 0, int retries = 0)
        {
            Retries = retries;
            CompensationMethod = compensationMethod;
            RetryDelayInMilliseconds = retryDelayInMilliseconds;
            Timeout = timeout;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _logger.Debug($"Initialized context {_omegaContext} before execution of method {args.Method.Name}");
            _recoveryPolicy.BeforeApply(_compensableInterceptor, _omegaContext, _omegaContext.GetLocalTxId(), Retries, Timeout, args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _recoveryPolicy.AfterApply(_compensableInterceptor, _omegaContext.GetLocalTxId(), args);
            _logger.Debug($"Transaction with context {_omegaContext} has finished.");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _logger.Error($"Transaction {_omegaContext.GetGlobalTxId()} failed.", args.Exception);
            _recoveryPolicy.ErrorApply(_compensableInterceptor, _omegaContext.GetLocalTxId(), args);
        }

    }
}
