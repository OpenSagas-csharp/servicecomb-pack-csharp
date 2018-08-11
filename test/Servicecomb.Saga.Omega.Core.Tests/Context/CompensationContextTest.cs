using System;
using Servicecomb.Saga.Omega.Core.Context;
using Xunit;

namespace Servicecomb.Saga.Omega.Core.Tests.Context
{
    public class CompensationContextTest
    {
        [Fact]
        public void Test()
        {
            var compensationContext=new CompensationContext();
            compensationContext.AddCompensationContext(GetType().GetMethod("DoSomething"), GetType());

            compensationContext.Apply(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), GetType().GetMethod("DoSomething").Name, null);
        }


        public string DoSomething()
        {
            return "doSomething";
        }
    }
}
