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

- .NET 8 or .NET 9 SDK

### Building the Project

1. Restore the NuGet packages:
```bash
dotnet restore
```
2. Build the solution:
```bash
dotnet build
```
### Usage
In your program.cs file add the following code for both ASP.NET apps and WASM applications.

```csharp
builder.AddTnTComponents();
```

If you are using an ASP.NET application with an API, you will need to add the [TnTComponents.AspNetCore](https://www.nuget.org/packages/TnTComponents.AspNetCore/) package to your project. This package provides custom bindings for virtualization and pagination of the data grid.

### Theming
Themes can be generated using Google's [Material 3 designer](https://material-foundation.github.io/material-theme-builder/). Export your theme as a json file and drop it in the `wwwroot` folder. 
Inside your `App.razor` file, add the following code:
```csharp
    <TnTComponents.TnTThemeDesign ThemeFile="{Name of your .json file}" />
```
Dark, light, and system themes can be applied by setting the `Theme` property of the `TnTThemeDesign` component.
## Contributing

Contributions are welcome!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
