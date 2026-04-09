# DI-From-Scratch

Build a simple Dependency Injection (DI) container from scratch in C# to learn how service registration, resolution, and lifetimes work.

---

## Features

- Register services with **Transient, Singleton, and Scoped** lifetimes
- Resolve services via **ServiceProvider**
- Uses **interfaces for abstraction** (`IServiceCollection`, `IServiceDescriptor`, `IServiceProvider`)
- Stores singleton instances automatically
- Simple, minimalistic design for learning purposes

---

## Project Structure
```
Solution/
├─ DI-From-Scratch/ (Console App / DI library)
│ ├─ Core/
│ │ ├─ ServiceCollection.cs
│ │ ├─ ServiceProvider.cs
│ ├─ Abstraction/
│ │ ├─ IServiceCollection.cs
│ │ ├─ IServiceProvider.cs
│ ├─ Utilities/
│ │ ├─ ServiceDescriptor.cs
│ │ └─ IServiceDescriptor.cs
│ ├─ Lifetime/
│ │ ├─ ServiceLifetimeAttribute.cs
│ │ └─ SingletonAttribute.cs
│ │ └─ TransientAttribute.cs
│ │ └─ ScopedAttribute.cs
│ └─ Program.cs (example usage)
│
├─ DI-From-Scratch.Tests/ (Class Library / xUnit tests)
│ ├─ ServiceCollectionTests.cs
│ └─ ServiceProviderTests.cs
```
## Usage Example

var services = new ServiceCollection();

services.AddSingleton<IUserService, UserService>();
services.AddTransient<IEmailService, EmailService>();

var provider = new ServiceProvider(services);

var userService1 = provider.GetService<IUserService>();
var userService2 = provider.GetService<IUserService>();

Console.WriteLine(userService1 == userService2 ? "Singleton works!" : "Oops!"); // Singleton test


## Lifetimes Explained

- **Transient**: creates a new instance every time `GetService` is called  
- **Singleton**: creates one instance and reuses it for all requests  
- **Scoped**: optional, simplified behavior depending on implementation  


## Advanced Features (DI From Scratch v2)

This version extends the container with real-world DI capabilities.

### 1. Auto Registration (Assembly Scanning)

Automatically registers services by scanning assemblies.

Supports:
- Multiple assemblies
- Lifetime selection
- Predicate filtering
- Flexible discovery rules

var services = new ServiceCollection();

services.AutoRegister(
    new[] { Assembly.GetExecutingAssembly() },
    ServiceLifetime.Singleton
);

### With Predicate Filtering

services.AutoRegister(
    new[] { Assembly.GetExecutingAssembly() },
    ServiceLifetime.Transient,
    t => t.Name.Contains("IP")
);

Use cases:
- Filtering interfaces
- Excluding internal types
- Controlling registration rules


### 2. Factory-Based Registration

Supports custom instance creation with logic or external configuration.

services.AddSingleton<IMessageService>(sp =>
    new EmailService("test@example.com")
);

Or direct instance:

var email = new EmailService("test@example.com");
services.AddSingleton<IMessageService>(email);

Use cases:
- Configuration-based services
- External SDK initialization
- Pre-built objects


### 3. Multiple Implementations (IEnumerable Support)

Supports resolving all implementations of an interface.

var validators = provider.GetService<IEnumerable<IValidator>>();

services.AddTransient<IValidator, EmailValidator>();
services.AddTransient<IValidator, PasswordValidator>();

Result:
- EmailValidator
- PasswordValidator


### 4. Service Release / Disposal

provider.Dispose();

Behavior:
- Disposes singleton services
- Logs release events
- Frees resources


## How to Run

1. Open solution in Visual Studio  
2. Set DI-From-Scratch as startup project  
3. Run Program.cs  
4. Run tests:

dotnet test


## Purpose

This project is educational and demonstrates:

- Service registration pipelines (manual + auto scanning)
- Dependency resolution engine
- Lifetime management (Singleton / Transient)
- Factory-based injection
- Multi-implementation resolution
- Resource cleanup lifecycle
- Reflection-based architecture design