using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Transaction.Exception
{
  public class InvalidTransactionException: System.Exception
  {
    public InvalidTransactionException(string message) : base(message)
    {

    }

    public InvalidTransactionException(string cause, System.Exception throwable) : base(cause, throwable)
    {
    }
  }
}
