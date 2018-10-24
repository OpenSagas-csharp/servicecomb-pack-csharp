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
using System.Reflection;
using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using ServiceLocator = Servicecomb.Saga.Omega.Abstractions.Transaction.Extensions.ServiceLocator;

namespace Servicecomb.Saga.Omega.Abstractions.Transaction
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Module)]
    public class SagaStartAttribute : Attribute
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SagaStartAttribute));

        private readonly ISagaStartEventAwareInterceptor _sagaStartAnnotationProcessor;

        private readonly OmegaContext _omegaContext;

        public int TimeOut { get; set; } = 0;

        protected object InitInstance;

        protected MethodBase InitMethod;

        protected Object[] Args;

        public void Init(object instance, MethodBase method, object[] args)
        {
            InitMethod = method;
            InitInstance = instance;
            Args = args;
        }

        public SagaStartAttribute()
        {
            _omegaContext = (OmegaContext)ServiceLocator.Current.GetInstance(typeof(OmegaContext));
            _sagaStartAnnotationProcessor =
                (ISagaStartEventAwareInterceptor)ServiceLocator.Current.GetInstance(typeof(ISagaStartEventAwareInterceptor));
        }


        public  void OnEntry()
        {
            InitializeOmegaContext();
            _sagaStartAnnotationProcessor.PreIntercept(_omegaContext.GetGlobalTxId(), InitMethod.Name, TimeOut, "", 0);
            _logger.Debug($"Initialized context {_omegaContext} before execution of method {InitMethod.Name}");
        }

        public  void OnExit( )
        {
            _sagaStartAnnotationProcessor.PostIntercept(_omegaContext.GetGlobalTxId(), InitMethod.Name);
            _logger.Debug($"Transaction with context {_omegaContext} has finished.");

        }

        public  void OnException(Exception exception)
        {
            _sagaStartAnnotationProcessor.OnError(null, "", exception);
            _logger.Error($"Transaction {_omegaContext.GetGlobalTxId()} failed.", exception);
        }

        private void InitializeOmegaContext()
        {
            _omegaContext.SetLocalTxId(_omegaContext.NewGlobalTxId());
        }
    }
}
