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

using System;
using ILogger = Servicecomb.Saga.Omega.Abstractions.Logging.ILogger;

namespace Servicecomb.Saga.Omega.Core.Logging.NLog
{
  public class NLogLogger : ILogger
  {
    private readonly ILogger _logger;
    public NLogLogger(ILogger logger)
    {
      _logger = logger;
    }
    public void Debug(string message)
    {
      _logger.Debug(message);
    }

    public void Info(string message)
    {
      _logger.Info(message);
    }

    public void Warning(string message)
    {
      _logger.Warning(message);
    }

    public void Error(string message, Exception exception)
    {
      _logger.Error(message, exception);
    }

    public void Trace(string message)
    {
      _logger.Trace(message);
    }
  }
}
