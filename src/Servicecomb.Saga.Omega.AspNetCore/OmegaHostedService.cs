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

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Transaction;
using Servicecomb.Saga.Omega.Protocol;

namespace Servicecomb.Saga.Omega.AspNetCore
{
    public class OmegaHostedService : IHostedService
    {
        private readonly TracingDiagnosticProcessorObserver _diagnosticObserver;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;

        public OmegaHostedService(TracingDiagnosticProcessorObserver tracingDiagnosticProcessorObserver, ILoggerFactory loggerFactory, IMessageSender messageSender)
        {
            _logger = loggerFactory.CreateLogger(typeof(OmegaHostedService));
            _diagnosticObserver = tracingDiagnosticProcessorObserver;
            _messageSender = messageSender;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Omega is connecting Alpha.");
            DiagnosticListener.AllListeners.Subscribe(_diagnosticObserver);
            var task = new Task(RunConnect);
            _logger.Debug("Omega is Disconnected Alpha.");
            return task;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            var task = new Task(RunDisConnect);
            _logger.Debug("Omega is Disconnected Alpha.");
            return task;
        }

        private void RunConnect()
        {
            DiagnosticListener.AllListeners.Subscribe(_diagnosticObserver);
            _messageSender.OnConnected();
        }

        private void RunDisConnect()
        {
            _messageSender.OnDisconnected();
        }
    }
}
