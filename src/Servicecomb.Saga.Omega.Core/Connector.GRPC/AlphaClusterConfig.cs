using System;
using System.Collections.Generic;
using System.Text;

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
