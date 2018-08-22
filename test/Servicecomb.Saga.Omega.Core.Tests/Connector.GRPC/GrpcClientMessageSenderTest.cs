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

using Grpc.Core;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Servicecomb.Saga.Omega.Core.Serializing;
using Servicecomb.Saga.Omega.Protocol;
using Xunit;

namespace Servicecomb.Saga.Omega.Core.Tests.Connector.GRPC
{
    public class GrpcClientMessageSenderTest
    {
        [Fact]
        public void CanRunGrpcClientMessageSender()
        {
            var grpcServiceConfig = new GrpcServiceConfig
            {
                InstanceId = "InstanceIdTest",
                ServiceName = "ServiceNameTest"
            };
            Assert.NotNull(grpcServiceConfig);
            Assert.Equal("InstanceIdTest", grpcServiceConfig.InstanceId);
            Assert.Equal("ServiceNameTest", grpcServiceConfig.ServiceName);
            
            var  channel=new Channel("localhost:8080",ChannelCredentials.Insecure);
            Assert.Equal("localhost:8080", channel.Target);


            var messageSerializer = new JsonMessageFormat();
            
            var grpcClientMessageSender=new GrpcClientMessageSender(grpcServiceConfig,channel,messageSerializer,channel.Target);
        
            Assert.NotNull(grpcClientMessageSender);
        }
    }
}
