using DI_From_Scratch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Abstractions
{
    public interface IServiceCollection
    {
        IReadOnlyDictionary<Type,IEnumerable<ServiceDescriptor>> ServiceDescriptors { get; }

        // Register Transient services
        void AddTransient<TRequest, TResponse>();
        void AddTransient<T>();

        // Register Scoped services
        void AddScoped<TRequest, TResponse>();
        void AddScoped<T>();

        // Register Singleton services
        void AddSingleton<TRequest, TResponse>();
        void AddSingleton<T>();
    }
}
