using System;

namespace Servicecomb.Saga.Omega.Abstractions.Logging
{
    public interface ILogger
    {
        void Debug(String message);

        void Info(String message);

        void Warning(String message);

        void Error(String message, Exception exception);

        void Trace(String message);
    }
}