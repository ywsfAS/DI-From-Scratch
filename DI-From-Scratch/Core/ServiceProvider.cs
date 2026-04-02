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
                return default;

            // 2. Handle Singleton lifetime
            if (service.ServiceLifetime == ServiceLifetime.Singleton)
            {
                if (service.Instance != null)
                    return (T)service.Instance;

                // Recursive constructor injection
                service.Instance = CreateInstance(service.ImplementationType);
                return (T)service.Instance;
            }

            // 3. Handle Transient lifetime (always create a new instance)
            return (T?)CreateInstance(service.ImplementationType);
        }

        // Helper method to handle constructor injection
        private object CreateInstance(Type implementationType)
        {
            var constructor = implementationType.GetConstructors().First();
            var parameters = constructor.GetParameters();

            // Resolve each dependency recursively
            var args = parameters.Select(p => GetServiceByType(p.ParameterType)).ToArray();

            return Activator.CreateInstance(implementationType, args)!;
        }

        // Non-generic GetService for runtime Type
        private object? GetServiceByType(Type type)
        {
            var method = typeof(ServiceProvider)
                .GetMethod(nameof(GetService), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                ?.MakeGenericMethod(type);

            return method?.Invoke(this, null);
        }
    }
}