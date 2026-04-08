using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Utilities
{
    public enum ServiceLifetime
    {
        Transient,
        Scoped,
        Singleton,
        All,
    }
}
