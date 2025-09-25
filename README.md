# TnTComponents
[![Deploy to NuGet](https://github.com/Notallthatevil/TnTComponents/actions/workflows/TnTComponentsToNuget.yml/badge.svg)](https://github.com/Notallthatevil/TnTComponents/actions/workflows/TnTComponentsToNuget.yml)

TnTComponents is a Blazor WebAssembly project that provides a set of reusable UI components for building modern web applications based on Google's Material 3 spec. The components are designed to be highly customizable and easy to use.

## Features

- **Form Components**: Includes various form components like `TnTInputFile` with advanced features.
- **Toast Notifications**: Provides a service for displaying toast notifications with different styles and messages.
- **Theming**: Supports theming with customizable color schemes and styles.
- **Grid**: A data grid component modified from FluentDataGrid.
- **Scheduler**: A scheduler component with week view and event management.

## Getting Started

### Prerequisites

- .NET 9 or .NET 10 SDK

### Install

Install from NuGet (package id: `TnTComponents`):

```
dotnet add package TnTComponents
```

Or add the package reference in your project file.

### Building the Project

1. Restore the NuGet packages:
```
dotnet restore
```
2. Build the solution:
```
dotnet build
```

### Usage
In your `Program.cs` file add the following to register any library services (see `LiveTest` for examples):

```csharp
// builder is the WebAssemblyHostBuilder or WebApplicationBuilder
builder.Services.AddTnTComponents();
```

Then use components in your pages (see `LiveTest` samples for exact component names and parameters):

```razor
@page "/"
<h3>Example</h3>
<TnTButton OnClick="() => Console.WriteLine("Clicked")">Click me</TnTButton>
```

### Theming
Themes can be generated using Google's Material 3 designer. Export your theme as a json file and drop it in the `wwwroot` folder. Inside your `App.razor` file, add the following code:

```razor
<TnTComponents.TnTThemeDesign ThemeFile="your-theme.json" />
```

Dark, light, and system themes can be applied by setting the `Theme` property of the `TnTThemeDesign` component.

## Contributing

Contributions are welcome! 

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

