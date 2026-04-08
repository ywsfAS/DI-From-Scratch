using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Lifetime
{
   public class TransientAttribute : ServiceLifetimeAttribute
   {
        public TransientAttribute() : base(ServiceLifetime.Transient) { }
   }
}
