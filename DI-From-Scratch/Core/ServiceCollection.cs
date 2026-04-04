using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
namespace DI_From_Scratch.Core
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type,ServiceDescriptor> _services;
        public IReadOnlyDictionary<Type,ServiceDescriptor> ServiceDescriptors => _services.AsReadOnly();
        public ServiceCollection()
        {
            _services = new Dictionary<Type,ServiceDescriptor>();
        }
        public void AddTransient<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Transient);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddTransient<TRequest>(Func<IServiceProviderDI , object> factory)
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest),typeof(object), ServiceLifetime.Transient , factory);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddTransient<T>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Transient);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }

        public void AddScoped<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Scoped);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddScoped<TRequest>(Func<IServiceProviderDI , object> factory)
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(object), ServiceLifetime.Scoped , factory);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddScoped<T>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Scoped);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }

        public void AddSingleton<TRequest,TResponse>()
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(TResponse)  ,ServiceLifetime.Singleton);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddSingleton<TRequest>(Func<IServiceProviderDI , object> factory)
        {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(TRequest) , typeof(object)  ,ServiceLifetime.Singleton , factory);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
        public void AddSingleton<T>() {
            var serviceDescriptor = ServiceDescriptor.Create(typeof(T) , typeof(T)  ,ServiceLifetime.Singleton);
            _services[serviceDescriptor.ServiceType] = serviceDescriptor;
        }
    }
}
