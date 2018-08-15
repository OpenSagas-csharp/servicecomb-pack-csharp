using System;
using System.Reflection;
using MethodBoundaryAspect.Fody.Attributes;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Transaction.Impl;

namespace Servicecomb.Saga.Omega.Core.Transaction
{
    public interface IRecoveryPolicy
    {
        void BeforeApply(CompensableInterceptor compensableInterceptor, OmegaContext context, String parentTxId, int retries, int timeout, string methodName,params object[] parameters);

        void AfterApply(CompensableInterceptor compensableInterceptor, string parentTxId, string methodName);

        void ErrorApply(CompensableInterceptor compensableInterceptor, string parentTxId, string methodName, System.Exception throwable);
    }
}
