using System;
using System.Collections.Generic;
using System.Text;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Transaction;
using Servicecomb.Saga.Omega.Protocol;

namespace Servicecomb.Saga.Omega.Core.Connector.GRPC
{
    public class GrpcClientMessageSender: IMessageSender
    {
        
        public void OnConnected()
        {
            throw new NotImplementedException();
        }

        public void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public string Target()
        {
            throw new NotImplementedException();
        }

        public AlphaResponse Send(TxEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
