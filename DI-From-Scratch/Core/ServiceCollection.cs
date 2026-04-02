using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
namespace DI_From_Scratch.Core
{
    public class ServiceCollection : IServiceCollection
    {
        private List<ServiceDescriptor> _services;
        public IReadOnlyList<ServiceDescriptor> ServiceDescriptors => _services.AsReadOnly();
        public ServiceCollection()
        {
            _services = new List<ServiceDescriptor>();
        }
        public void AddTransient<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Transient);
            _services.Add(serviceDescriptor);
        }
        public void AddTransient<T>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Transient);
            _services.Add(serviceDescriptor);
        }

        public void AddScoped<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Scoped);
            _services.Add(serviceDescriptor);
        }
        public void AddScoped<T>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Scoped);
            _services.Add(serviceDescriptor);
        }

        public void AddSingleton<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Singleton);
            _services.Add(serviceDescriptor);
        }

        public void AddSingleton<T>() {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Singleton);
            _services.Add(serviceDescriptor);
        }
    }
}
