using DI_From_Scratch.Abstractions;
using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Core
{
    // Meta data about the service to inject later
    public class ServiceDescriptor : IServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public ServiceLifetime ServiceLifetime { get; }

        public Func<IServiceProviderDI, object>? ServiceFactory { get; } = null; // to handle special dependency creation
        public object? Instance { get; set; } = null; // to handle reference to instance 
        private ServiceDescriptor(Type serviceType , Type implementationType , ServiceLifetime serviceLifetime , Func<IServiceProviderDI , object>? factory = null)
        {
            ServiceLifetime = serviceLifetime;
            ServiceType = serviceType;
            ImplementationType = implementationType;
            ServiceType = serviceType;
            ServiceFactory = factory;

        }
        public static ServiceDescriptor Create(Type serviceType , Type implementationType , ServiceLifetime lifetime , Func<IServiceProviderDI , object>? factory = null)
        {
            return new ServiceDescriptor(serviceType , implementationType , lifetime , factory);
        }

    }
}
