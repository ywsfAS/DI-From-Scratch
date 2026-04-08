using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
using DI_From_Scratch.Lifetime;
using System.Collections.Generic;
using System.Reflection;

namespace DI_From_Scratch.Core
{
    public class ServiceCollection : IServiceCollection  , IDisposable
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _services;

        public IReadOnlyDictionary<Type, IEnumerable<ServiceDescriptor>> ServiceDescriptors =>
            _services.ToDictionary(kvp => kvp.Key, kvp => (IEnumerable<ServiceDescriptor>)kvp.Value);

        public ServiceCollection()
        {
            _services = new Dictionary<Type, List<ServiceDescriptor>>();
        }

        private void AddDescriptor(ServiceDescriptor descriptor)
        {
            if (!_services.ContainsKey(descriptor.ServiceType))
                _services[descriptor.ServiceType] = new List<ServiceDescriptor>();

            _services[descriptor.ServiceType].Add(descriptor);
        }

        public void AddTransient<TRequest, TResponse>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(TResponse), ServiceLifetime.Transient);
            AddDescriptor(descriptor);
        }
        public void AddTransient(Type TRequest , Type TResponse)
        {
            var descriptor = ServiceDescriptor.Create(TRequest, TResponse, ServiceLifetime.Transient);
            AddDescriptor(descriptor);

        }

        public void AddTransient<TRequest>(Func<IServiceProviderDI, object> factory)
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(object), ServiceLifetime.Transient, factory);
            AddDescriptor(descriptor);
        }

        public void AddTransient<T>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(T), typeof(T), ServiceLifetime.Transient);
            AddDescriptor(descriptor);
        }

        public void AddScoped<TRequest, TResponse>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(TResponse), ServiceLifetime.Scoped);
            AddDescriptor(descriptor);
        }

        public void AddScoped(Type TRequest , Type TResponse)
        {
            var descriptor = ServiceDescriptor.Create(TRequest, TResponse, ServiceLifetime.Scoped);
            AddDescriptor(descriptor);
        }

        public void AddScoped<TRequest>(Func<IServiceProviderDI, object> factory)
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(object), ServiceLifetime.Scoped, factory);
            AddDescriptor(descriptor);
        }

        public void AddScoped<T>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(T), typeof(T), ServiceLifetime.Scoped);
            AddDescriptor(descriptor);
        }

        public void AddSingleton<TRequest, TResponse>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(TResponse), ServiceLifetime.Singleton);
            AddDescriptor(descriptor);
        }
        public void AddSingleton(Type TRequest , Type TResponse)
        {
            var descriptor = ServiceDescriptor.Create(TRequest, TResponse, ServiceLifetime.Singleton);
            AddDescriptor(descriptor);
        }
        public void AddSingleton<TRequest>(Func<IServiceProviderDI, object> factory)
        {
            var descriptor = ServiceDescriptor.Create(typeof(TRequest), typeof(object), ServiceLifetime.Singleton, factory);
            AddDescriptor(descriptor);
        }

        public void AddSingleton<T>()
        {
            var descriptor = ServiceDescriptor.Create(typeof(T), typeof(T), ServiceLifetime.Singleton);
            AddDescriptor(descriptor);
        }

        public void AddSingleton(object instance)
        {
            var type = instance.GetType();
            var descriptor = ServiceDescriptor.Create(type, type, ServiceLifetime.Singleton, sp => instance);
            descriptor.Instance = instance;
            AddDescriptor(descriptor);
        }
        private void Register(Type serviceType , Type implementationType,ServiceLifetime type)
        {
            switch (type)
            {
                case ServiceLifetime.Singleton:
                    AddSingleton(serviceType, implementationType);
                    break;

                case ServiceLifetime.Scoped:
                    AddScoped(serviceType, implementationType);
                    break;

                case ServiceLifetime.Transient:
                    AddTransient(serviceType, implementationType);
                    break;
            }
        }
        // Register the container by assembly
        public void AutoRegister(Assembly[] assemblies, ServiceLifetime? lifetime = ServiceLifetime.All, Predicate<Type>? predicate = null)
        {
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type implementationType in assembly.GetTypes())
                {
                    // Skip invalid types
                    if (!implementationType.IsClass || implementationType.IsAbstract)
                        continue;

                    // Get lifetime attribute
                    var lifetimeAttr = implementationType
                        .GetCustomAttribute<ServiceLifetimeAttribute>(false);

                    if (lifetimeAttr is null || (lifetime != ServiceLifetime.All && lifetimeAttr.ServiceLifetime != lifetime))
                        continue;

                    var interfaces = implementationType.GetInterfaces();

                    // Apply predicate safely
                    var serviceTypes = predicate != null
                        ? interfaces.Where(i => predicate(i))
                        : interfaces;

                    foreach (var serviceType in serviceTypes)
                    {
                        Register(serviceType, implementationType, lifetimeAttr.ServiceLifetime);
                    }
                }
            }
        }
        public void Dispose()
        {
            foreach (var service in _services)
            {
                var serviceDecriptorsList = service.Value;
                foreach(var serviceType in serviceDecriptorsList)
                {
                    var instance = serviceType.Instance;
                    if(instance != null && instance is IDisposable disposableInstance)
                    {
                        disposableInstance.Dispose();
                    }
                }
            } 
        }
    } }