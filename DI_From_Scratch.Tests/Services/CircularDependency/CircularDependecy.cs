using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests.Services.CircularDependency
{
    class classA
    {
        private classB _b ;
        public classA(classB b)
        {
            _b = b;
        }
    }
    class classB
    {
        private classC _c;
        public classB(classC c)
        {
            _c = c;
        }
    }
    class classC
    {
       private classA _a;
        public classC(classA a)
        {
            _a = a;
        }
    }
}

