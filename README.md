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
- **Scoped**: optional, can mimic singleton for simplicity  

---

## How to Run

1. Open solution in Visual Studio  
2. Set `DI-From-Scratch` as startup project  
3. Run `Program.cs` to see example usage  
4. Optional: add a test project and run `dotnet test` for unit tests  

---

## Purpose

This project is **educational**, helping you understand:

- How DI containers work under the hood  
- Abstraction via interfaces  
- Managing object lifetimes  
- Clean architecture principles in practice
