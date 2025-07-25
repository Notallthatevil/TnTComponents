
# Copilot Instructions for TnTComponents

## Context-Specific Guidance
### 0: Most Important Instruction
- Never assume anything about the project structure or conventions. Always follow the project's established patterns and conventions. If unsure, ask for clarification.

### 1. Code Generation
- For any prompt that requires generating or modifying code, first consult [`.github/instructions/generated-code.md`](./instructions/generated-code.md) for project-specific code generation rules, patterns, and examples.
### 2. Testing
- For any prompt that requires writing tests, first consult [`.github/instructions/testing.md`](./instructions/unit-tests.md) for project-specific testing rules, patterns, and examples.

### 3. General Project Guidance
- For code style guidelines, refer to [`.github/instructions/coding-style.md`](./instructions/code-style.md).
- **Project Overview**: TnTComponents is a Blazor WebAssembly library of reusable UI components, following Google's Material 3 spec.
- **Solution Structure**:
  - `TnTComponents/`: Core component library (e.g., Accordion, Badge, Buttons, Grid, Scheduler, etc.)
  - `LiveTest/`: Example/test Blazor app for development and manual testing.
  - `TnTComponents.AspNetCore/`: ASP.NET Core integration for server-side features (e.g., data grid virtualization).
  - `TnTComponents.Extensions/`: Utility extensions.
- **Component Patterns**: Each component has its own folder with `.razor`, `.razor.cs`, `.razor.css`, `.razor.scss`, and sometimes `.js` files. Example: `TnTComponents/Buttons/TnTButton.razor`.
- **Theming**: Themes are JSON files (exported from Material Theme Builder) placed in `wwwroot/Themes`. By adding a `TnTComponents.TnTThemeToggle` component in an app, users can switch themes dynamically.
- **Testing**: Tests are organized by component in `TnTComponents.Tests/`, mirroring the main library structure.

### 4. Developer Workflows
- **Build**: `dotnet restore` then `dotnet build` at the solution root.
- **Test**: `dotnet test` from the solution root or target specific test projects.
- **Run Example App**: Launch `LiveTest/LiveTest` for manual component testing.
- Always use the command line for building and testing to ensure consistency.

### 5. Integration Points
- **ASP.NET Core**: Add the `TnTComponents.AspNetCore` NuGet package for server-side features.

### 6. Project-Specific Guidance
- **Component APIs**: Follow the established patterns for parameters, events, and cascading values as seen in current components.
- **Styling**: Use `.razor.scss` for component styles; compiled CSS is included in `.razor.css` and `.razor.min.css`.
- **Extending Components**: Place new components in their own folder under `TnTComponents/`, following the established file structure.

### 7. Documentation 
- When documenting properties, do not describe it as getting or setting a value, instead describe what the property is or how it affects the behavior.
- If a property, method, or field is inherited or implmented from a base class or interface, use the `inheritdoc` tag to inherit documentation from the base class or interface.