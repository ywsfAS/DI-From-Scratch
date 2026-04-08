using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services;
using DI_From_Scratch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests
{
    public class AutoRegistrationTests
    {
        public AutoRegistrationTests() { }
        [Fact]
        public void AutoRegistration_Should_Register_Only_Singleton() {
            // Arrange
            var collection = new ServiceCollection();
            collection.AutoRegister(new[] { Assembly.GetExecutingAssembly() }, ServiceLifetime.Singleton);
            var provider = new ServiceProvider(collection);

            // Act 
            var user = provider.GetService<IUserService>();
            var transientService1 = provider.GetService<IPaymentService>();
            var transientService2 = provider.GetService<IOrderService>();

            // Assert
            Assert.NotNull(user);
            Assert.IsType<UserService>(user);
            Assert.Null(transientService1);
            Assert.Null(transientService2);


        }
        [Fact]
        public void AutoRegistration_Should_Register_Only_Transient()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AutoRegister(new[] { Assembly.GetExecutingAssembly() }, ServiceLifetime.Transient);
            var provider = new ServiceProvider(collection);

            // Act 
            var singleonService = provider.GetService<IUserService>();
            var transientService1 = provider.GetService<IPaymentService>();
            var transientService2 = provider.GetService<IOrderService>();

            // Assert
            Assert.NotNull(transientService2);
            Assert.NotNull(transientService2);
            Assert.IsType<PaymentService>(transientService1);
            Assert.IsType<OrderService>(transientService2);
            Assert.Null(singleonService);


        }
        [Fact]
        public void AutoRegistration_Should_Register_All()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AutoRegister(new[] { Assembly.GetExecutingAssembly() });
            var provider = new ServiceProvider(collection);

            // Act 
            var singleonService = provider.GetService<IUserService>();
            var transientService1 = provider.GetService<IPaymentService>();
            var transientService2 = provider.GetService<IOrderService>();

            // Assert
            Assert.NotNull(transientService1);
            Assert.NotNull(transientService2);
            Assert.NotNull(singleonService);
            Assert.IsType<PaymentService>(transientService1);
            Assert.IsType<OrderService>(transientService2);
            Assert.IsType<UserService>(singleonService);


        }


        [Fact]
        public void AutoRegistration_Should_Register_By_Predicate()
        {
            // Arrange
            var collection = new ServiceCollection();
            collection.AutoRegister(new[] { Assembly.GetExecutingAssembly() },ServiceLifetime.Transient,(t) => t.Name.Contains("IP"));
            var provider = new ServiceProvider(collection);

            // Act 
            var singleonService = provider.GetService<IUserService>();
            var transientService1 = provider.GetService<IPaymentService>();
            var transientService2 = provider.GetService<IOrderService>();

            // Assert
            Assert.NotNull(transientService1);
            Assert.IsType<PaymentService>(transientService1);

            Assert.Null(transientService2);
            Assert.Null(singleonService);


        }
    }
}
