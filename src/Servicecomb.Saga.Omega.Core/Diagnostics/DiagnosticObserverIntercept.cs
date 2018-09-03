using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DiagnosticAdapter;
using Servicecomb.Saga.Omega.Abstractions.Diagnostics;

namespace Servicecomb.Saga.Omega.Core.Diagnostics
{
    public class DiagnosticObserverIntercept : IObserver<KeyValuePair<string, object>>
    {
        private readonly IDiagnosticItercept _diagnosticItercept;
        private readonly Dictionary<string, MethodInfo> _dictionary;
        private readonly Dictionary<string, string> _diagnosticNameDictionary = new Dictionary<string, string>()
        {
            { "Microsoft.AspNetCore.Hosting.BeginRequest","httpContext"},
            {"System.Net.Http.Request", "Request"},
        };
        private static readonly object _object = new object();
        public DiagnosticObserverIntercept(IDiagnosticItercept diagnosticItercept)
        {
            _diagnosticItercept = diagnosticItercept;
            _dictionary = new Dictionary<string, MethodInfo>();
            Inint();


        }

        public void OnCompleted()
        {
            // just do nothing here
        }

        public void OnError(Exception error)
        {
            // just do nothing here
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            lock (_object)
            {
                if (!_dictionary.ContainsKey(value.Key)) return;
                var method = _dictionary.First(x => x.Key == value.Key).Value;
                var property = value.Value.GetType().GetProperty(_diagnosticNameDictionary[value.Key]);
                object[] arg = { property?.GetValue(value.Value) };
                method.Invoke(_diagnosticItercept, arg);
            }
            
        }

        void Inint()
        {


            var assembly = Assembly.GetCallingAssembly();
            var methodInfos = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(DiagnosticNameAttribute), false).Length > 0)
                .ToList();
            foreach (var method in methodInfos)
            {
                var attribute = method.GetCustomAttribute<DiagnosticNameAttribute>();
                if (attribute == null) continue;
                _dictionary.Add(attribute.Name, method);

            }
        }
    }
}
