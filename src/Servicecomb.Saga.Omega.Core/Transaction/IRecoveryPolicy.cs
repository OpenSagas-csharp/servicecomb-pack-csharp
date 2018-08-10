using System;
using MethodBoundaryAspect.Fody.Attributes;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public interface IRecoveryPolicy
    {
        void BeforeApply(CompensableInterceptor compensableInterceptor, OmegaContext context, String parentTxId, int retries, int timeout, MethodExecutionArgs args);

        void AfterApply(CompensableInterceptor compensableInterceptor, string parentTxId, MethodExecutionArgs args);

        void ErrorApply(CompensableInterceptor compensableInterceptor, string parentTxId, MethodExecutionArgs args);
    }
}
