using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;
using Servicecomb.Saga.Omega.Abstractions.Logging;
using Servicecomb.Saga.Omega.Core.Context;
using Servicecomb.Saga.Omega.Core.Diagnostics;
using Servicecomb.Saga.Omega.Core.Logging;

namespace Servicecomb.Saga.Omega.Core.Transport.HttpClient
{
    public class HttpClientDiagnosticProcessor:ITracingDiagnosticProcessor
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(HttpClientDiagnosticProcessor));
        public string ListenerName { get; } = "HttpHandlerDiagnosticListener";

        [DiagnosticName("System.Net.Http.Request")]
        public void HttpRequest([Property(Name = "Request")] HttpRequestMessage request)
        {

        }

        [DiagnosticName("System.Net.Http.Response")]
        public void HttpResponse([Property(Name = "Response")] HttpResponseMessage response)
        {
        }

        [DiagnosticName("System.Net.Http.Exception")]
        public void HttpException([Property(Name = "Request")] HttpRequestMessage request,
            [Property(Name = "Exception")] Exception ex)
        {

        }
    }
}
