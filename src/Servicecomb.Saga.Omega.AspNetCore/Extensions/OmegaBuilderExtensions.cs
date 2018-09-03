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
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.DependencyInjection;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Serializing;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;
using Servicecomb.Saga.Omega.Core.Transport.AspNetCore;
using Servicecomb.Saga.Omega.Protocol;

namespace Servicecomb.Saga.Omega.AspNetCore.Extensions
{
    public static class OmegaBuilderExtensions
    {
        public static OmegaBuilder AddHosting(this OmegaBuilder builder, Action<OmegaOptions> options)
        {
            builder.Services.AddSingleton<IHostedService, OmegaHostedService>();
            builder.Services.AddSingleton<IMessageSerializer, JsonMessageFormat>();
            builder.Services.AddSingleton<IDiagnosticItercept, HostingDiagnosticIntercept>();
            builder.Services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            builder.Services.AddSingleton(typeof(IIdGenerator<string>), typeof(UniqueIdGenerator));
            builder.Services.AddSingleton<IMessageHandler, CompensationMessageHandler>();
            builder.Services.AddSingleton<IRecoveryPolicy, DefaultRecovery>();
            builder.Services.AddSingleton<OmegaContext>();
            builder.Services.AddSingleton<CompensationContext>();
            builder.Services.AddSingleton<SagaStartAttribute>();
            var option = new OmegaOptions();
            options(option);
            builder.Services.AddSingleton<IMessageSender>(new GrpcClientMessageSender(
                new GrpcServiceConfig
                {

                    InstanceId = option.InstanceId,
                    ServiceName = option.ServiceName
                },
                new Channel(option.GrpcServerAddress, ChannelCredentials.Insecure),
                new JsonMessageFormat(),
                option.GrpcServerAddress
                ));

            builder.Services.AddSingleton<IEventAwareInterceptor, SagaStartAnnotationProcessor>();
            builder.Services.AddSingleton<IEventAwareInterceptor, CompensableInterceptor>();
            builder.Services.AddSingleton<ISagaStartEventAwareInterceptor>(services =>
                new SagaStartAnnotationProcessor(services.GetService<OmegaContext>(), services.GetService<IMessageSender>()));

            return builder;
        }

        public static OmegaBuilder AddDiagnostics(this OmegaBuilder builder)
        {
            builder.Services.AddSingleton<DiagnosticListenerObserver>();
            return builder;
        }
    }
}
