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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;

namespace Servicecomb.Saga.Omega.Core.Diagnostics
{
    public class TracingDiagnosticMethod
    {
        private readonly MethodInfo _method;
        private readonly ITracingDiagnosticProcessor _diagnosticProcessor;
        private readonly string _diagnosticName;
        private readonly IParameterResolver[] _parameterResolvers;

        public TracingDiagnosticMethod(ITracingDiagnosticProcessor diagnosticProcessor, MethodInfo method,
            string diagnosticName)
        {
            _method = method;
            _diagnosticProcessor = diagnosticProcessor;
            _diagnosticName = diagnosticName;
            _parameterResolvers = GetParameterResolvers(method).ToArray();
        }

        public void Invoke(string diagnosticName, object value)
        {
            if (_diagnosticName != diagnosticName)
            {
                return;
            }

            var args = new object[_parameterResolvers.Length];
            for (var i = 0; i < _parameterResolvers.Length; i++)
            {
                args[i] = _parameterResolvers[i].Resolve(value);
            }

            _method.Invoke(_diagnosticProcessor, args);

        }

        private static IEnumerable<IParameterResolver> GetParameterResolvers(MethodBase methodBase)
        {
            foreach (var parameter in methodBase.GetParameters())
            {
                var binder = parameter.GetCustomAttribute<ParameterBinder>();
                if (binder != null)
                {
                    switch (binder)
                    {
                        case ObjectAttribute objectBinder:
                            if (objectBinder.TargetType == null)
                            {
                                objectBinder.TargetType = parameter.ParameterType;
                            }

                            break;
                        case PropertyAttribute propertyBinder:
                            if (propertyBinder.Name == null)
                            {
                                propertyBinder.Name = parameter.Name;
                            }

                            break;
                    }

                    yield return binder;
                }
                else
                {
                    yield return new NullParameterResolver();
                }
            }
        }
    }
}
