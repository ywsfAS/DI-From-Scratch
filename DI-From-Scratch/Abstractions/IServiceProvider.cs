using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Abstractions
{
    public interface IServiceProviderDI
    {
        // Resolve an instance of type T
        T? GetService<T>();
    }
}
