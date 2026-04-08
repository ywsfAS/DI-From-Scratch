using System;
using System.Collections.Generic;
using System.Linq;
using DI_From_Scratch.Utilities;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Lifetime
{
    public class ScopedAttribute : ServiceLifetimeAttribute
    {
        public ScopedAttribute() : base(ServiceLifetime.Scoped){ }
    }
}
