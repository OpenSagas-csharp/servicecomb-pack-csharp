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
using System.Collections.Concurrent;
using System.Reflection;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transaction.Extensions;

namespace Servicecomb.Saga.Omega.Core.Context
{
    public class CompensationContext
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(CompensationContext));
        private readonly ConcurrentDictionary<string, CompensationContextInternal> _contexts =
                new ConcurrentDictionary<string, CompensationContextInternal>();

        public void AddCompensationContext(MethodInfo compensationMethod, Type target)
        {
            _contexts.TryAdd(compensationMethod.Name, new CompensationContextInternal(target, compensationMethod));
        }

        public void Apply(string globalTxId, string localTxId, string compensationMethod, params byte[] payloads)
        {
            CompensationContextInternal contextInternal = null;
            try
            {
                _contexts.TryGetValue(compensationMethod, out contextInternal);
                var classInstance = Activator.CreateInstance(contextInternal?.Target ?? throw new InvalidOperationException(), null);
                var messageFormat = (IMessageSerializer)ServiceLocator.Current.GetInstance(typeof(IMessageSerializer));
                var result = messageFormat.Deserialize<dynamic>(payloads);
                contextInternal.CompensationMethod.Invoke(classInstance, null);
                _logger.Info($"Compensated transaction with global tx id [{globalTxId}], local tx id [{localTxId}]");


            }
            catch (TargetInvocationException ex)
            {
                _logger.Info(
                  $"Compensated transaction with global tx id [{globalTxId}], local tx id .error(Pre-checking for compensation method {contextInternal?.CompensationMethod.Name} was somehow skipped, did you forget to configure compensable method checking on service startup?{ex}");

            }
        }


        public class CompensationContextInternal
        {
            public Type Target;

            public MethodInfo CompensationMethod;

            public CompensationContextInternal(Type target, MethodInfo compensationMethod)
            {
                Target = target;
                CompensationMethod = compensationMethod;
            }
        }
    }


}
