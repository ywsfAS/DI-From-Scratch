using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
using System.Collections.Generic;
using System.Reflection;

namespace DI_From_Scratch.Core
{
    public class ServiceProvider : IServiceProviderDI , IDisposable
    {
        // The collection of registered service descriptors
        private readonly ServiceCollection _servicesCollection;

        public ServiceProvider(ServiceCollection servicesCollection)
        {
            _servicesCollection = servicesCollection;
        }
        // Public entry point
        public T? GetService<T>()
        {
            var hashSet = new HashSet<Type>();
            var constructorCache = new Dictionary<Type, ConstructorInfo>();

            return (T?)resolver(typeof(T), hashSet, constructorCache);
        }

        // Look up the descriptor for a given type T
        private ServiceDescriptor? Lookup(Type serviceType)
        {
            if (_servicesCollection.ServiceDescriptors.TryGetValue(serviceType, out var list) && list.Any())
            {
                return list.Last();
            }

            return null;
        }
        // Handle IEnumerable<T> dependencies
        private object? HandleCollectionDependency(Type serviceType, HashSet<Type> hashSet, Dictionary<Type, ConstructorInfo> constructorCache)
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var innerType = serviceType.GetGenericArguments()[0];
                var method = typeof(ServiceProvider)
                    .GetMethod(nameof(GetServices), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(innerType);

                return method.Invoke(this, new object[] { hashSet, constructorCache });
            }
            return null;
        }
        // Get all services for a registered type
        private IEnumerable<T> GetServices<T>(HashSet<Type> hashSet, Dictionary<Type, ConstructorInfo> constructorCache)
        {
            var type = typeof(T);
            if (!_servicesCollection.ServiceDescriptors.TryGetValue(type, out var descriptors))
                yield break;

            foreach (var descriptor in descriptors)
            {
                if (descriptor.ServiceLifetime == ServiceLifetime.Singleton)
                {
                    if (descriptor.Instance == null)
                        descriptor.Instance = CreateInstance(descriptor.ImplementationType, hashSet, constructorCache);

                    yield return (T)descriptor.Instance!;
                }
                else
                {
                    yield return (T)CreateInstance(descriptor.ImplementationType, hashSet, constructorCache);
                }
            }
        }
        // Core resolver
        private object? resolver(Type serviceType, HashSet<Type> hashSet, Dictionary<Type, ConstructorInfo> constructorInfos)
        {
            // Handle IEnumerable<T>
            var collectionResult = HandleCollectionDependency(serviceType, hashSet, constructorInfos);
            if (collectionResult != null)
                return collectionResult;

            // Lookup the service descriptor
            ServiceDescriptor? service = Lookup(serviceType);


            if (service is null)
            {
                if (!serviceType.IsAbstract)
                    return CreateInstance(serviceType, hashSet, constructorInfos);

                return null;
            }
            DetectCircularDependencyAndThrowError(hashSet, service.ServiceType);

            try
            {
                // Singleton
                if (service.ServiceLifetime == ServiceLifetime.Singleton)
                {
                    if (service.Instance != null)
                        return service.Instance;

                    if (service.ServiceFactory != null)
                    {
                        service.Instance = service.ServiceFactory(this);
                        return service.Instance;
                    }

                    service.Instance = CreateInstance(service.ImplementationType, hashSet, constructorInfos);
                    return service.Instance;
                }

                // Transient with factory
                if (service.ServiceFactory != null)
                    return service.ServiceFactory(this);

                // Transient with constructor
                return CreateInstance(service.ImplementationType, hashSet, constructorInfos);
            }
            finally
            {
                hashSet.Remove(service?.ServiceType ?? serviceType);
            }
        }


        // Circular dependency detection
        private void DetectCircularDependencyAndThrowError(HashSet<Type> hashSet, Type current)
        {
            if (hashSet.Contains(current))
                throw new InvalidOperationException($"Circular Dependency detected in {current.Name}");

            hashSet.Add(current);
        }

        // Constructor injection
        private object CreateInstance(Type implementationType, HashSet<Type> hashSet, Dictionary<Type, ConstructorInfo> constructorInfos)
        {
            if (!constructorInfos.TryGetValue(implementationType, out var constructor))
            {
                constructor = implementationType.GetConstructors()
                    .OrderByDescending(c => c.GetParameters().Length)
                    .First();
                constructorInfos.Add(implementationType, constructor);
            }

            var parameters = constructor.GetParameters();

            var args = parameters.Select(p =>
            {
                return resolver(p.ParameterType, hashSet, constructorInfos);
            }).ToArray();

            return Activator.CreateInstance(implementationType, args)!;
        }
        // Release phase
        public void Dispose()
        {
            _servicesCollection.Dispose();
        }
    }
}