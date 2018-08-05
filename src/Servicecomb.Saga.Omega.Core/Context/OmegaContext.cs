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

using Servicecomb.Saga.Omega.Abstractions.Context;
using System;
using System.Threading;

namespace Servicecomb.Saga.Omega.Core.Context
{
    public class OmegaContext
    {
        public static readonly string _global_tx_id_key = "X-Pack-Global-Transaction-Id";
        public static readonly string _local_tx_id_key = "X-Pack-Local-Transaction-Id";
        private ThreadLocal<string> GlobalTxId = new ThreadLocal<string>();
        private ThreadLocal<string> LocalTxId = new ThreadLocal<string>();
        private IIdGenerator<String> IdGenerator;

        public OmegaContext(IIdGenerator<string> idGenerator)
        {
            this.IdGenerator = idGenerator;
        }

        public string NewGlobalTxId()
        {
            var id = IdGenerator.nextId();
            GlobalTxId.Value = id;
            return id;
        }

        public void SetGlobalTxId(string txId)
        {
            GlobalTxId.Value = txId;
        }

        public string NewLocalTxId()
        {
            String id = IdGenerator.nextId();
            LocalTxId.Value = id;
            return id;
        }

        public string localTxId()
        {
            return LocalTxId.Value;
        }

        public void clear()
        {
            //globalTxId.remove();
            //localTxId.remove();
            GlobalTxId.Value = null;
            LocalTxId.Value = null;
        }

        public override string ToString()
        {
            return $"OmegaContext{"{"}globalTxId={GlobalTxId.Value},localTxId={LocalTxId.Value}{"}"}";
        }
    }
}
