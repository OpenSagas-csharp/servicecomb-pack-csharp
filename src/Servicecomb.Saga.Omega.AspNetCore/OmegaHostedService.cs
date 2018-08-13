using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Logging;
using Servicecomb.Saga.Omega.Core.Transaction;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;
using Servicecomb.Saga.Omega.Protocol;

namespace Servicecomb.Saga.Omega.AspNetCore
{
    public class OmegaHostedService : IHostedService
    {
        private readonly TracingDiagnosticProcessorObserver _diagnosticObserver;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;

        public OmegaHostedService(TracingDiagnosticProcessorObserver tracingDiagnosticProcessorObserver, IOptions<OmegaOptions> options, IMessageSerializer serializer, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(OmegaHostedService));
            _diagnosticObserver = tracingDiagnosticProcessorObserver;
            _messageSender = new GrpcClientMessageSender(
                new GrpcServiceConfig()
                {
                    ServiceName = options.Value.ServiceName,
                    InstanceId = options.Value.InstanceId
                },
                new Channel(options.Value.GrpcServerAddress, ChannelCredentials.Insecure), serializer, options.Value.GrpcServerAddress);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Omega is connecting Alpha.");
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
