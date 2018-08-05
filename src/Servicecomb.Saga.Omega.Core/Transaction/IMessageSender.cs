using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public interface IMessageSender
    {
        void OnConnected();

        void OnDisconnected();

        void Close();

        string Target();

        AlphaResponse Send(TxEvent @event);
        }
}
