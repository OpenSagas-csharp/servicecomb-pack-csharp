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

using Microsoft.Extensions.DependencyInjection;
using Servicecomb.Saga.Omega.Abstractions.Context;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Servicecomb.Saga.Omega.Core.DependencyInjection;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Serializing;
using Servicecomb.Saga.Omega.Core.Transaction;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;
using Servicecomb.Saga.Omega.Core.Transport.AspNetCore;

namespace Servicecomb.Saga.Omega.AspNetCore.Extensions
{
    public static class OmegaBuilderExtensions
    {
        public static OmegaBuilder AddHosting(this OmegaBuilder builder)
        {
            builder.Services.AddSingleton<IMessageSerializer, MessagePackMessageFormat>();
            builder.Services.AddSingleton<ITracingDiagnosticProcessor, HostingDiagnosticProcessor>();
            builder.Services.AddSingleton<ILoggerFactory, ILoggerFactory>();
            builder.Services.AddSingleton<IIdGenerator<string>>();
            builder.Services.AddSingleton<IMessageHandler, CompensationMessageHandler>();
            builder.Services.AddSingleton<IEventAwareInterceptor>();
            builder.Services.AddSingleton<IMessageSender, GrpcClientMessageSender>();
            builder.Services.AddSingleton<IRecoveryPolicy, DefaultRecovery>();
            return builder;
        }

        public static OmegaBuilder AddDiagnostics(this OmegaBuilder builder)
        {
            builder.Services.AddSingleton<TracingDiagnosticProcessorObserver>();
            return builder;
        }
    }
}
