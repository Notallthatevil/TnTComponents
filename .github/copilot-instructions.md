# Copilot Instructions for TnTComponents

## Overview
TnTComponents is a Blazor WebAssembly and ASP.NET Core component library focused on reusable, customizable UI elements based on Google's Material 3 spec. The project is organized as a multi-project solution with core components in `TnTComponents/`, ASP.NET Core bindings in `TnTComponents.AspNetCore/`, and tests in `TnTComponents.Tests/`.

## Architecture & Structure
- **Component Organization:**
  - Each component (e.g., Accordion, Badge, Button) resides in its own folder under `TnTComponents/`.
  - Components typically include `.razor`, `.razor.cs`, `.razor.css`, `.razor.scss`, and sometimes `.js` files.
  - Shared logic and interfaces are in `TnTComponents/Core/` and `TnTComponents/Interfaces/`.
- **Grid Component:**
  - The data grid in `TnTComponents/Grid/` is a modified version of FluentDataGrid (see `Grid/README.md`).
- **Theming:**
  - Themes are generated via Material 3 designer and loaded using the `TnTThemeDesign` component in your `App.razor`.
  - Place theme JSON files in the `wwwroot` folder.

## Developer Workflows
- **Build:**
  - Restore and build with:
    ```powershell
    dotnet restore
    dotnet build
    ```
- **Test:**
  - Tests are in `TnTComponents.Tests/`. Run with:
    ```powershell
    dotnet test TnTComponents.Tests/TnTComponents.Tests.csproj
    ```
- **NuGet Publishing:**
  - CI/CD is configured via GitHub Actions (`TnTComponentsToNuget.yml`).

## Project-Specific Conventions
- **Component Naming:**
  - All components are prefixed with `TnT` (e.g., `TnTButton`, `TnTAccordion`).
- **Partial Classes:**
  - Use `.razor` for markup and `.razor.cs` for logic (code-behind pattern).
- **Styling:**
  - Use `.razor.scss` for authoring styles; compiled CSS is checked in as `.razor.css`.
- **Extensibility:**
  - Many components are designed for extensibility via parameters and child content.
- **ASP.NET Core Integration:**
  - Use the `TnTComponents.AspNetCore` package for server-side features like grid virtualization.

## Integration & Dependencies
- **External:**
  - Relies on .NET 8/9 SDK and Blazor.
  - Grid component is based on FluentDataGrid (MIT-licensed).
- **Internal:**
  - Cross-component communication is typically via cascading parameters or event callbacks.

## Key Files & Directories
- `TnTComponents/` — Main component library
- `TnTComponents.AspNetCore/` — ASP.NET Core integration
- `TnTComponents.Tests/` — Test suite
- `TnTComponents/Grid/README.md` — Grid component details
- `README.md` — Project overview and getting started

## Examples
- **Registering Components:**
  ```csharp
  builder.AddTnTComponents();
  ```
- **Applying a Theme:**
  ```csharp
  <TnTComponents.TnTThemeDesign ThemeFile="mytheme.json" />
  ```

---

For more details, see the main `README.md` and component-specific folders.
