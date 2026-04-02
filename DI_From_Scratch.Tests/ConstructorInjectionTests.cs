using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services;
using DI_From_Scratch.Utilities;
using Xunit;
public class ConstructorInjectionTests
{
    [Fact]
    public void GetService_ShouldInjectConstructorDependencies()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<PaymentService>();
        services.AddTransient<OrderService>();

        var provider = new ServiceProvider(services);

        // Act
        var orderService = provider.GetService<OrderService>();

        // Assert
        Assert.NotNull(orderService); // OrderService was created
        Assert.NotNull(orderService!.PaymentService); // PaymentService was injected
        Assert.IsType<PaymentService>(orderService.PaymentService); // Correct type
    }
}
