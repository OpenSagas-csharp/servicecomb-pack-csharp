/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
