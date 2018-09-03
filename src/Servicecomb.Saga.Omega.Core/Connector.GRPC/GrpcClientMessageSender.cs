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

using System.Text;
using System.Threading;
using Google.Protobuf;
using Grpc.Core;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Protocol;
using ServiceLocator = Servicecomb.Saga.Omega.Abstractions.Transaction.Extensions.ServiceLocator;

namespace Servicecomb.Saga.Omega.Core.Connector.GRPC
{
    public class GrpcClientMessageSender : IMessageSender
    {
        private readonly GrpcServiceConfig _serviceConfig;
        private readonly TxEventService.TxEventServiceClient _client;
        private readonly IMessageSerializer _serializer;
        private readonly string _target;
        private IMessageHandler _messageHandler => (IMessageHandler)ServiceLocator.Current.GetInstance(typeof(IMessageHandler));

        public GrpcClientMessageSender(GrpcServiceConfig serviceConfig, Channel channel, IMessageSerializer serializer, string address)
        {
            _serviceConfig = serviceConfig;
            _client = new TxEventService.TxEventServiceClient(channel);
            _serializer = serializer;
            _target = address;

        }

        public async void OnConnected()
        {
            var command = _client.OnConnected(_serviceConfig);
            while (await command.ResponseStream.MoveNext(CancellationToken.None))
            {

                _messageHandler.OnReceive(command.ResponseStream.Current.GlobalTxId, command.ResponseStream.Current.LocalTxId, command.ResponseStream.Current.ParentTxId, command.ResponseStream.Current.CompensationMethod, command.ResponseStream.Current.Payloads.ToByteArray());
            }

        }

        public void OnDisconnected()
        {
            _client.OnDisconnected(_serviceConfig);
        }

        public void Close()
        {
            // just do nothing here
        }

        public string Target()
        {
            return _target;
        }

        public AlphaResponse Send(TxEvent @event)
        {
            var grpcAck = _client.OnTxEvent(ConvertEvent(@event));
            return new AlphaResponse(grpcAck.Aborted);
        }

        private GrpcTxEvent ConvertEvent(TxEvent @event)
        {

            var payloads = ByteString.CopyFrom(_serializer.Serialize(@event.Payloads), Encoding.UTF8);
            return new GrpcTxEvent()
            {
                ServiceName = _serviceConfig.ServiceName,
                InstanceId = _serviceConfig.InstanceId,
                Timestamp = @event.Timestamp,
                GlobalTxId = @event.GlobalTxId,
                LocalTxId = @event.LocalTxId,
                ParentTxId = @event.ParentTxId ?? "",
                Type = @event.Type.ToString(),
                Timeout = @event.Timeout,
                CompensationMethod = @event.CompensationMethod,
                RetryMethod = @event.RetryMethod ?? "",
                Retries = @event.Retries,
                Payloads = payloads
            };
        }
    }
}
