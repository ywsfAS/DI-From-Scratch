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
        private ServiceDescriptor? Lookup(Type serviceType)
        {
            if (_servicesCollection.ServiceDescriptors.ContainsKey(serviceType)) { 
                return _servicesCollection.ServiceDescriptors[serviceType];
            }
            return null;
            
        }

        // Resolve an instance of type T
        public T? GetService<T>()
        {
            var hashSet = new HashSet<Type>();
            var constructorCache = new Dictionary<Type, ConstructorInfo>();
            return (T?)resolver(typeof(T),hashSet , constructorCache);
        }
        private object? resolver(Type serviceType ,HashSet<Type> hashSet , Dictionary<Type , ConstructorInfo> constructorInfos)
        {
            // 1. Find the service descriptor
            ServiceDescriptor? service = Lookup(serviceType);
            if (service is null)
                return null;
            DetectCircularDependecyAndThrowError(hashSet,service.ServiceType);
            try
            {
                // 2. Handle Singleton lifetime
                if (service.ServiceLifetime == ServiceLifetime.Singleton)
                {
                    if (service.Instance != null)
                        return service.Instance;

                    // Recursive constructor injection
                    service.Instance = CreateInstance(service.ImplementationType, hashSet , constructorInfos);

                    return service.Instance;
                }
                // Other Cases
                return CreateInstance(service.ImplementationType, hashSet , constructorInfos);
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
        private object CreateInstance(Type implementationType , HashSet<Type> hashSet , Dictionary<Type , ConstructorInfo> constructorInfos)
        {
            ConstructorInfo constrcutor;
            if (constructorInfos.ContainsKey(implementationType))
            {
                constrcutor = constructorInfos[implementationType];
            }
            else
            {
                constrcutor = implementationType.GetConstructors().OrderByDescending( c => c.GetParameters().Length).First();
                constructorInfos.Add(implementationType, constrcutor);
            }
            var parameters = constrcutor.GetParameters();

            // Resolve each dependency recursively
            var args = parameters.Select(p => resolver(p.ParameterType ,hashSet , constructorInfos)).ToArray();

            return Activator.CreateInstance(implementationType, args)!;
        }
    }
}