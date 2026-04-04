using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services;
using DI_From_Scratch.Tests.Services.FactoryDependecy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests
{
    public class FactoryDependencyInjectionTests
    {
        public FactoryDependencyInjectionTests() { 
        
        }
        [Fact]
        public void FactoryDependencyInjection_ShouldInjectDependency()
        {
            // Arrange
            var services = new ServiceCollection();

            // Factory registration for IMessageService
            services.AddSingleton<IMessageService>(sp => new EmailService("HoaaaASX@example.com"));

            // Normal constructor injection for NotificationService
            services.AddSingleton<NotificationService>();

            var provider = new ServiceProvider(services);

            // Act
            var emailService = provider.GetService<IMessageService>();
            var notificationService = provider.GetService<NotificationService>();

            // Assert

            // Ensure emailService is not null
            Assert.NotNull(emailService);

            // Ensure notificationService is not null
            Assert.NotNull(notificationService);

            // Ensure the injected dependency is the same instance (singleton)
            var notificationField = typeof(NotificationService)
                .GetField("_messageService", BindingFlags.NonPublic | BindingFlags.Instance);

            var injectedService = notificationField?.GetValue(notificationService);

            Assert.Same(emailService, injectedService);
        }
        
        [Fact]
        public void RegisterDependencyInjection_ShouldInjectDependency()
        {
            // Arrange
            var email = new EmailService("HoaaaASX@example.com");
            var services = new ServiceCollection();

            // Factory registration for IMessageService
            services.AddSingleton(email);


            var provider = new ServiceProvider(services);

            // Act
            var emailService = provider.GetService<EmailService>();

            // Assert

            // Ensure emailService is not null
            Assert.NotNull(emailService);

            // Ensure notificationService is not null
            Assert.Same(email,emailService);


        }
    }
}
