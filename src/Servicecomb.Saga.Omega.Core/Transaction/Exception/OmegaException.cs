using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction.Exception
{
    public class OmegaException : System.Exception
    {
        public OmegaException(string message) : base(message)
        {

        }

        public OmegaException(string cause, System.Exception throwable) : base(cause, throwable)
        {
        }
    }
}
