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


using ArxOne.MrAdvice.Advice;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public class SagaStartAspect:SagaStartAttribute, IMethodAdvice
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SagaStartAspect));

        private readonly SagaStartAnnotationProcessor _sagaStartAnnotationProcessor;

        private readonly OmegaContext _omegaContext;

        public SagaStartAspect(IMessageSender sender, OmegaContext context)
        {
            _omegaContext = context;
            _sagaStartAnnotationProcessor = new SagaStartAnnotationProcessor(context, sender);
        }

        public void Advise(MethodAdviceContext context)
        {
            InitializeOmegaContext();
            _sagaStartAnnotationProcessor.PreIntercept(_omegaContext.GetGlobalTxId(), context.TargetMethod.Name, TimeOut, "", 0);
            _logger.Debug($"Initialized context {context} before execution of method {context.TargetMethod.Name}");

            try
            {

                context.Proceed();
                _sagaStartAnnotationProcessor.PostIntercept(_omegaContext.GetGlobalTxId(), context.TargetMethod.Name);
                _logger.Debug($"Transaction with context {context} has finished.");
            }
            catch (System.Exception throwable)
            {
                _logger.Error($"Transaction {_omegaContext.GetGlobalTxId()} failed.", throwable);
                throw;
            }
            finally
            {
                _omegaContext.Clear();
            }
        }

        private void InitializeOmegaContext()
        {
            _omegaContext.SetLocalTxId(_omegaContext.NewGlobalTxId());
        }
    }
}
