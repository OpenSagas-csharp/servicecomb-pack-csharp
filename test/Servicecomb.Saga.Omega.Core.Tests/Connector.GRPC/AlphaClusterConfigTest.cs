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

using System.Collections.Generic;
using Servicecomb.Saga.Omega.Core.Connector.GRPC;
using Xunit;

namespace Servicecomb.Saga.Omega.Core.Tests.Connector.GRPC
{
    public class AlphaClusterConfigTest
    {
        [Fact]
        public void CanCreateAlphaClusterConfig()
        {
            var alphaClusterConfig =
                new AlphaClusterConfig(new List<string>() {"localhost"}, true, true, "123", "456", "certChain");

            Assert.Equal("localhost", alphaClusterConfig.Addresses[0]);
            Assert.True(alphaClusterConfig.EnableSsl);
            Assert.True(alphaClusterConfig.EnableMutualAuth);
            Assert.Equal("123", alphaClusterConfig.Cert);
            Assert.Equal("456", alphaClusterConfig.Key);
            Assert.Equal("certChain", alphaClusterConfig.CertChain);

        }
    }
}
