using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services.CircularDependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests
{
    public class CircularDependecyTests
    {
        [Fact]
        public void GetService_ShouldThowExceptionCircularDependecyChain()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AddTransient<classA>();
            collection.AddTransient<classB>();
            collection.AddTransient<classC>();

            var provider = new ServiceProvider(collection);
            // Act and Assert
            var ex = Assert.Throws<InvalidOperationException>(() => provider.GetService<classA>());
            Assert.Contains("Circular Dependency", ex.Message);

        }
    }
}
