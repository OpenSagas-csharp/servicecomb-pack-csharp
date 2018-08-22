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
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Abstractions.Transaction.Extensions;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;
using ServiceLocator = Servicecomb.Saga.Omega.Core.Transaction.Exception.ServiceLocator;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SagaStartAttribute: OnMethodBoundaryAspect
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SagaStartAttribute));

        private readonly SagaStartAnnotationProcessor _sagaStartAnnotationProcessor;

        private readonly OmegaContext _omegaContext;

        public int TimeOut { get; set; } = 0;

        public SagaStartAttribute()
        {
            _omegaContext = (OmegaContext)ServiceLocator.Current.GetInstance(typeof(OmegaContext));
            _sagaStartAnnotationProcessor = new SagaStartAnnotationProcessor(_omegaContext, (IMessageSender)ServiceLocator.Current.GetInstance(typeof(IMessageSender))) ;
        }


        public override void OnEntry(MethodExecutionArgs args)
        {
            InitializeOmegaContext();
            _sagaStartAnnotationProcessor.PreIntercept(_omegaContext.GetGlobalTxId(), args.Method.Name, 0, "", 0);
            _logger.Debug($"Initialized context {_omegaContext} before execution of method {args.Method.Name}");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _sagaStartAnnotationProcessor.PostIntercept(_omegaContext.GetGlobalTxId(), args.Method.Name);
            _logger.Debug($"Transaction with context {_omegaContext} has finished.");
            
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _sagaStartAnnotationProcessor.OnError(null,"",args.Exception);
            _logger.Error($"Transaction {_omegaContext.GetGlobalTxId()} failed.", args.Exception);
        }

        private void InitializeOmegaContext()
        {
            _omegaContext.SetLocalTxId(_omegaContext.NewGlobalTxId());
        }
    }
}
