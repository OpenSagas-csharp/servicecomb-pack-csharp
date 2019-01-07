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

//FROMhttps://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/DiagnosticSourceUsersGuide.md
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Core.Utils;

namespace Servicecomb.Saga.Omega.Core.Diagnostics
{
    public class DiagnosticListenerObserver : IObserver<DiagnosticListener>
    {
        private readonly IEnumerable<IDiagnosticItercept> _diagnosticItercepts;
        private static readonly object _object = new object();

        public DiagnosticListenerObserver(IEnumerable<IDiagnosticItercept> diagnosticItercepts)
        {
            _diagnosticItercepts = diagnosticItercepts;
        }

        public void OnCompleted()
        {
            // just do nothing here
        }

        public void OnError(Exception error)
        {
            // just do nothing here
        }

        public void OnNext(DiagnosticListener diagnosticListener)
        {
            var result = _diagnosticItercepts.Distinct(x => x.ListenerName).ToList()
                .Find(x => x.ListenerName == diagnosticListener.Name);
            if (result == null) return;
            //The DiagnosticSource/DiagnosticListener code is thread safe, but the callback code also needs to be thread safe.
            lock (_object)
            {
                diagnosticListener.Subscribe(new DiagnosticObserverIntercept(result));
            }
        }

    }
}
