using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Abstractions
{
    public interface IServiceDescriptor
    {
        // Interface for service metadata
        public interface IServiceDescriptor
        {
            Type ServiceType { get; }
            Type ImplementationType { get; }
            ServiceLifetime ServiceLifetime { get; }
            object? Instance { get; set; }
        }
    }
}
