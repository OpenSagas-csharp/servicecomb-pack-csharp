using System;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
  public interface IRecoveryPolicy
  {
    Object Apply(CompensableInterceptor compensableInterceptor, OmegaContext context, String parentTxId, int retries,
      CompensableAttribute compensable,
      string compentationMethod);
  }
}
