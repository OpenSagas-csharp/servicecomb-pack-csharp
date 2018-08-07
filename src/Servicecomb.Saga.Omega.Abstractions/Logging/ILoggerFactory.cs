using System;

namespace Servicecomb.Saga.Omega.Abstractions.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}