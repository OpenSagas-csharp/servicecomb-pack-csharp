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


using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transport.HttpClient;

namespace Servicecomb.Saga.Omega.Core.Transport.AspNetCore
{
    public class HostingDiagnosticProcessor: ITracingDiagnosticProcessor
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(HttpClientDiagnosticProcessor));
        private readonly OmegaContext _omegaContext;

        public HostingDiagnosticProcessor(OmegaContext omegaContext)
        {
            _omegaContext = omegaContext;
        }
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        [DiagnosticName("Microsoft.AspNetCore.Hosting.BeginRequest")]
        public void BeginRequest([Property] Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (_omegaContext == null) return;
            string globalTxId = httpContext.Request.Headers[OmegaContext.GlobalTxIdKey];
            if (globalTxId == null)
            {
                _logger.Debug($"no such header: {OmegaContext.GlobalTxIdKey}");
            }
            else
            {
                _omegaContext.SetGlobalTxId(globalTxId);
                _omegaContext.SetLocalTxId(httpContext.Request.Headers[OmegaContext.GlobalTxIdKey]);
            }

        }
    }
}
