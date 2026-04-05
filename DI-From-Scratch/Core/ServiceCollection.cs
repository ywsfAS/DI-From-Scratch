using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;

namespace DI_From_Scratch.Core
{
    public class ServiceCollection : IServiceCollection
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
    }
}