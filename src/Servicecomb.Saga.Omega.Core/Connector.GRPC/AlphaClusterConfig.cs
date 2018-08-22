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
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied .
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;

namespace Servicecomb.Saga.Omega.Core.Connector.GRPC
{
    public class AlphaClusterConfig
    {
        public List<string> Addresses { get; set; }

        public bool EnableSsl { get; set; }

        public bool EnableMutualAuth { get; set; }

        public string Cert { get; set; }

        public string Key { get; set; }

        public string CertChain { get; set; }

        public AlphaClusterConfig(List<string> addresses,
            bool enableSsl,
            bool enableMutualAuth,
            string cert,
            string key,
            string certChain)
        {
            Addresses = addresses;
            EnableMutualAuth = enableMutualAuth;
            EnableSsl = enableSsl;
            Cert = cert;
            Key = key;
            CertChain = certChain;
        }
    }
}
