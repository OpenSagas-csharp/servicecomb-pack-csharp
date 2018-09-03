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

using System.Net.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using ServiceLocator = Servicecomb.Saga.Omega.Abstractions.Transaction.Extensions.ServiceLocator;

namespace Servicecomb.Saga.Omega.Core.Transport.HttpClient
{
    public class HttpClientDiagnosticIntercept : IDiagnosticItercept
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(HttpClientDiagnosticIntercept));
        private readonly OmegaContext _omegaContext;
        public string ListenerName { get; } = "HttpHandlerDiagnosticListener";

        public HttpClientDiagnosticIntercept()
        {
            _omegaContext = (OmegaContext)ServiceLocator.Current.GetInstance(typeof(OmegaContext));
        }

        [DiagnosticName("System.Net.Http.Request")]
        public void Request(HttpRequestMessage request)
        {
            if (_omegaContext?.GetGlobalTxId() == null) return;
            request.Headers.Add(OmegaContext.GlobalTxIdKey, _omegaContext.GetGlobalTxId());
            request.Headers.Add(OmegaContext.LocalTxIdKey, _omegaContext.GetLocalTxId());
            _logger.Debug(
                $"Added {OmegaContext.GlobalTxIdKey} {_omegaContext.GetGlobalTxId()} and {OmegaContext.LocalTxIdKey} {_omegaContext.GetLocalTxId()} to request header");
        }
    }
}
