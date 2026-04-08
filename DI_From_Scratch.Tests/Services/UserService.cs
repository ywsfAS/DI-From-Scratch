using DI_From_Scratch.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests.Services
{
    [Singleton]
    public class UserService : IUserService , IDisposable
    {
        private readonly Guid _id;

        public UserService()
        {
            _id = Guid.NewGuid();
        }

        public string GetUserName() => $"UserService-{_id}";

        public Guid Id => _id;
        public void Dispose() {
            Console.WriteLine($"Release Rescources of instance{GetUserName()}");
        }
    }
}
