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
using Microsoft.Extensions.Hosting;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Diagnostics;

namespace Servicecomb.Saga.Omega.AspNetCore
{
    public class OmegaHostedService : IHostedService
    {
        private readonly DiagnosticListenerObserver _diagnosticObserver;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;

        public OmegaHostedService(DiagnosticListenerObserver tracingDiagnosticProcessorObserver, ILoggerFactory loggerFactory, IMessageSender messageSender)
        {
            _logger = loggerFactory.CreateLogger(typeof(OmegaHostedService));
            _diagnosticObserver = tracingDiagnosticProcessorObserver;
            _messageSender = messageSender;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Omega is connecting Alpha.");
            var task = new Task(RunConnect);
            task.Start();
            _logger.Debug("Omega is Disconnected Alpha.");
            return task;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            var task = new Task(RunDisConnect);
            task.Start();
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
