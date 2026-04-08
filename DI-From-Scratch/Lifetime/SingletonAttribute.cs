using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DI_From_Scratch.Utilities;
namespace DI_From_Scratch.Lifetime
{
    public class SingletonAttribute : ServiceLifetimeAttribute
    {
        public SingletonAttribute() : base(ServiceLifetime.Singleton) { }
    }
}
