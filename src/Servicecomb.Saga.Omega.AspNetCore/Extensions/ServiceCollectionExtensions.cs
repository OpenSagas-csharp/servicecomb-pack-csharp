using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Servicecomb.Saga.Omega.Core.DependencyInjection;
using Servicecomb.Saga.Omega.Core.Transport.HttpClient;

namespace Servicecomb.Saga.Omega.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static OmegaBuilder AddOmegaCore(this IServiceCollection services,
            Action<OmegaOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return services.Configure(options).AddOmegaCore();
        }
        private static OmegaBuilder AddOmegaCore([NotNull]this IServiceCollection services)
        {
            var builder = new OmegaBuilder(services);
            builder.AddHosting().AddDiagnostics().AddHttpClient();
            return builder;
        }
    }
}
