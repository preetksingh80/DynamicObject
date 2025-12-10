DynamicObject
=============

Experiments With Dynamic object

## Building and Running

This project has been modernized to use .NET 8.0.

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Build
```bash
dotnet build ConsoleApplication1/ConsoleApplication1.csproj
```

### Run
```bash
dotnet run --project ConsoleApplication1/ConsoleApplication1.csproj
```

### Expected Output
```
Preet
35
[Indercepted] person's age in json age:35
```

The application demonstrates:
- Dynamic object decoration
- Property interception
- Function member interception
