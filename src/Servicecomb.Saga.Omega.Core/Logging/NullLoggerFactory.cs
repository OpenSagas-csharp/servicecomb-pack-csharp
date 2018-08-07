using System;
using Servicecomb.Saga.Omega.Abstractions.Logging;

namespace Servicecomb.Saga.Omega.Core.Logging
{
    internal class NullLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new NullLogger();
        }
    }
}