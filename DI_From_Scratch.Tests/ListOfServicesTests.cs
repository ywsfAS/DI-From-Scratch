using DI_From_Scratch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests
{
    public interface IValidator { }
    public class EmailValidator : IValidator { }
    public class PasswordValidator : IValidator { }
    public class ListOfServicesTests
    {
        [Fact]
        public void GetServices_ShouldReturnAllRegisteredImplementations()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AddTransient<IValidator, EmailValidator>();
            collection.AddTransient<IValidator, PasswordValidator>();

            var provider = new ServiceProvider(collection);

            // Act
            var services = provider.GetService<IEnumerable<IValidator>>();

            // Assert
            Assert.NotNull(services);

            var list = new List<IValidator>(services!);
            Assert.Equal(2, list.Count); // Expect 2 implementations
            Assert.Contains(list, s => s.GetType() == typeof(EmailValidator));
            Assert.Contains(list, s => s.GetType() == typeof(PasswordValidator));
        }
    }
}
