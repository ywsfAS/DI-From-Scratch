using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;

namespace DI_From_Scratch.Core
{
    public class ServiceProvider : IServiceProviderDI
    {
        // The collection of registered service descriptors
        private readonly ServiceCollection _servicesCollection;

        public ServiceProvider(ServiceCollection servicesCollection)
        {
            _servicesCollection = servicesCollection;
        }

        // Look up the descriptor for a given type T
        private ServiceDescriptor? Lookup<T>()
        {
            foreach (var service in _servicesCollection.ServiceDescriptors)
            {
                if (service.ServiceType == typeof(T))
                    return service;
            }
            return null;
        }

        // Resolve an instance of type T
        public T? GetService<T>()
        {
            // 1. Find the service descriptor
            ServiceDescriptor? service = Lookup<T>();
            if (service is null)
            {
                // Service not registered
                return default;
            }

            // 2. Handle Singleton lifetime
            if (service.ServiceLifetime == ServiceLifetime.Singleton)
            {
                // If instance already exists, return it
                if (service.Instance != null)
                    return (T)service.Instance;

                // Otherwise, create it and store for future
                service.Instance = Activator.CreateInstance(service.ImplementationType);
                return (T)service.Instance;
            }

            // 3. Handle Transient lifetime (always create a new instance)
            object? instance = Activator.CreateInstance(service.ImplementationType);
            return (T?)instance;
        }
    }
}