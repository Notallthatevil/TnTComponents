# TnTComponents Code Style Guide

## General

- Use `namespace` declarations that match the folder structure (e.g., `TnTComponents.Wizard`).
- Blazor components will be part of the TnTComponents namespace.
- Place one top-level type per file.
- Use `using` directives at the top of the file, outside the namespace.

## Classes & Members

- Use `public`, `private`, `protected`, and `internal` access modifiers explicitly.
- Use `abstract`, `virtual`, and `override` appropriately for extensibility.
- Prefer `readonly` for fields that should not change after construction.
- Use PascalCase for class, property, and method names.
- Use camelCase for private fields and parameters.
- Use `[Parameter]` and `[EditorRequired]` attributes for Blazor component parameters.
- Place XML documentation comments (`/// <summary> ... </summary>`) on all classes and members.
- Use `/// <inheritdoc />` where a member, method, or property is overidden or implemented from an interface.

## Formatting

- Use 4 spaces for indentation.
- Place opening braces `{` on the same line.
- End all statements with a semicolon `;`.
- Use expression-bodied members (`=>`) for simple properties or methods when appropriate.

## Comments

- Use `//` for comments.
- Use XML comments for public APIs.

## Naming

- Use descriptive names for classes, methods, and properties.
- Prefix interfaces with `I` (e.g., `IService`).
- Suffix async methods with `Async`.

## Blazor/Component Specific

- Use `[Parameter]` for all component parameters.
- Use `[CascadingParameter]` for cascading values.
- Use `[Inject]` for dependency injection.
- USe `.razor.cs` partial classes for code-behind for all code, ask for permission to insert an `@code` block in the `.razor` file.
- Keep components focused and testable—each should do one thing well.
