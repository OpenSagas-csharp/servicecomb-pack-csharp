using System;
using System.Collections.Generic;
using System.Text;
using Servicecomb.Saga.Omega.Core.Context;
using Xunit;

namespace Servicecomb.Saga.Omega.Core.Tests.Context
{
    public class OmegaContextTest
    {
        [Fact]
        public void CanGetId()
        {
            var omegaContext=new OmegaContext(new UniqueIdGenerator());
            omegaContext.NewGlobalTxId();
            omegaContext.NewLocalTxId();
            Assert.NotEmpty(omegaContext.GetGlobalTxId());
            Assert.NotEmpty(omegaContext.GetGlobalTxId());
            omegaContext.Clear();
        }
    }
}
