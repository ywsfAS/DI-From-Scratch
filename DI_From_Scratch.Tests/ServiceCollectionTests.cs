using DI_From_Scratch.Core;
using DI_From_Scratch.Utilities;
using DI_From_Scratch.Tests.Services;
using Xunit;

public class ServiceCollectionTests
{
    [Fact]
    public void Can_Add_Transient_Service()
    {
        var services = new ServiceCollection();
        services.AddTransient<IUserService, UserService>();

        Assert.Single(services.ServiceDescriptors);
        var descriptor = services.ServiceDescriptors[typeof(IUserService)].First();
        Assert.Equal(typeof(IUserService), descriptor.ServiceType);
        Assert.Equal(typeof(UserService), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.ServiceLifetime);
    }
}
