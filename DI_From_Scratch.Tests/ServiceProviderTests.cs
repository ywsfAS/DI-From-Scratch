using DI_From_Scratch.Core;
using DI_From_Scratch.Tests.Services;
using DI_From_Scratch.Utilities;
using Xunit;

public class ServiceProviderTests
{
    private ServiceProvider BuildProviderWithTransient()
    {
        var services = new ServiceCollection();
        services.AddTransient<IUserService, UserService>();
        return new ServiceProvider(services);
    }

    private ServiceProvider BuildProviderWithSingleton()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IUserService, UserService>();
        return new ServiceProvider(services);
    }

    [Fact]
    public void Transient_Service_Should_Create_New_Instance_Each_Time()
    {
        var provider = BuildProviderWithTransient();

        var first = provider.GetService<IUserService>();
        var second = provider.GetService<IUserService>();

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotEqual(((UserService)first).Id, ((UserService)second).Id);
    }

    [Fact]
    public void Singleton_Service_Should_Return_Same_Instance()
    {
        var provider = BuildProviderWithSingleton();

        var first = provider.GetService<IUserService>();
        var second = provider.GetService<IUserService>();

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(((UserService)first).Id, ((UserService)second).Id);
    }

    [Fact]
    public void Unregistered_Service_Should_Return_Null()
    {
        var services = new ServiceCollection();
        var provider = new ServiceProvider(services);

        var service = provider.GetService<IUserService>();
        Assert.Null(service);
    }
}
