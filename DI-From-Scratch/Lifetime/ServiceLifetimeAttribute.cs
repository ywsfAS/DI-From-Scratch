using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Lifetime
{
    public abstract class ServiceLifetimeAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }
        public ServiceLifetimeAttribute(ServiceLifetime lifetime)
        {
            ServiceLifetime = lifetime;
        }

    }
}
