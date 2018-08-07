using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Abstractions.Transaction
{
    public interface IMessageSerializer
    {
        byte[] Serialize(Object[] objects);
    }
}
