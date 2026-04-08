using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services;
using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests
{
    public class ReleaseRegistrationTests
    {
        [Fact]
        public void Release_Singleton_Should_Dispose()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AutoRegister(new[] { Assembly.GetExecutingAssembly() }, ServiceLifetime.Singleton, (t) => t.Name.Contains("IUser"));
            var provider = new ServiceProvider(collection);

            // Act 
            var user = provider.GetService<IUserService>();
            // Redirect from console to stringWriter buffer
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Assert
            Assert.NotNull(user);
            Assert.IsType<UserService>(user);
            provider.Dispose();
            string output = writer.ToString();
            Assert.Contains("Release",output);
        }

    }

}
