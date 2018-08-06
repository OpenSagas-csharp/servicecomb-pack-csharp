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

namespace Servicecomb.Saga.Omega.Core.Context
{
    public class CompensationContext
    {
        //private static final Logger LOG = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
        private ConcurrentDictionary<string, CompensationContextInternal> _contexts =
            new ConcurrentDictionary<string, CompensationContextInternal>();

        public void AddCompensationContext(MethodInfo compensationMethod, Object target)
        {
            _contexts.TryAdd(compensationMethod.Name, new CompensationContextInternal(target, compensationMethod));
        }

        public void Apply(string globalTxId, string localTxId, string compensationMethod, params Object[] payloads)
        {
            try
            {
                CompensationContextInternal contextInternal;
                _contexts.TryGetValue(compensationMethod, out contextInternal);
                contextInternal.CompensationMethod.Invoke(contextInternal.Target, payloads);
                //LOG.info("Compensated transaction with global tx id [{}], local tx id [{}]", globalTxId, localTxId);
            }
            catch (TargetInvocationException ex) 
            {
                //LOG.error(
                //    "Pre-checking for compensation method " + contextInternal.compensationMethod.toString()
                //                                            + " was somehow skipped, did you forget to configure compensable method checking on service startup?",
                //    e);
                
            }
        }


        public  class CompensationContextInternal
        {
            public  Object Target;

            public MethodInfo CompensationMethod;

            public CompensationContextInternal(Object target, MethodInfo compensationMethod)
            {
                this.Target = target;
                this.CompensationMethod = compensationMethod;
            }
        }
    }


}
