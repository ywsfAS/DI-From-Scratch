using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
using System.Reflection;

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
            if (!_servicesCollection.ServiceDescriptors.ContainsKey(typeof(T)))
            {
                return null;
            }
            return _servicesCollection.ServiceDescriptors[typeof(T)];
        }

        // Resolve an instance of type T
        public T? GetService<T>()
        {
            var hashSet = new HashSet<Type>();
            return (T?)resolver<T>(hashSet);
        }
        private object? resolver<T>(HashSet<Type> hashSet)
        {
            // 1. Find the service descriptor
            ServiceDescriptor? service = Lookup<T>();
            if (service is null)
                return null;
            DetectCircularDependecyAndThrowError(hashSet,service.ServiceType);
            try
            {
                // 2. Handle Singleton lifetime
                if (service.ServiceLifetime == ServiceLifetime.Singleton)
                {
                    if (service.Instance != null)
                        return (T)service.Instance;

                    // Recursive constructor injection
                    service.Instance = CreateInstance(service.ImplementationType, hashSet);

                    return (T)service.Instance;
                }
                // Other Cases
                return (T?)CreateInstance(service.ImplementationType, hashSet);
            }
            finally
            {
                hashSet.Remove(service.ServiceType);
            }
        }
        private void DetectCircularDependecyAndThrowError(HashSet<Type> hashSet , Type current)
        {
            if (hashSet.Contains(current))
            {
                throw new InvalidOperationException($"Detected Circular Dependency in {nameof(current.Name)}");
            }
            hashSet.Add(current);
        }

        // Helper method to handle constructor injection
        private object CreateInstance(Type implementationType , HashSet<Type> hashSet)
        {
            var constructor = implementationType.GetConstructors().First();
            var parameters = constructor.GetParameters();

            // Resolve each dependency recursively
            var args = parameters.Select(p => GetServiceByType(p.ParameterType ,hashSet)).ToArray();

            return Activator.CreateInstance(implementationType, args)!;
        }

        // Non-generic GetService for runtime Type
        private object? GetServiceByType(Type type, HashSet<Type> hashSet)
        {
            var method = typeof(ServiceProvider)
                .GetMethod(nameof(resolver), BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException($"Cannot find resolver method for type {type.Name}");

            method = method.MakeGenericMethod(type);

            try
            {
                return method.Invoke(this, new object[] { hashSet });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!; // unwrap the real exception
            }
        }
    }
}