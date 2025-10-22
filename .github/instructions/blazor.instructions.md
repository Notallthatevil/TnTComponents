---
applyTo: "**/*.razor"
---

The following rules describe conventions and requirements for building Blazor components in this repository. They apply to any file that matches the `applyTo` pattern above (i.e. `*.razor` files).

- Single responsibility: each component should do one thing and do it well. Keep components small and focused to make them easy to understand and test.

- No inline `@code` blocks: do not use a `@code` section inside `.razor` files. All component logic must live in a C# code-behind file named with the component, e.g. `MyComponent.razor.cs`.

- Dependency injection: avoid using `@inject` in `.razor` files. Prefer constructor injection in the code-behind class (use a primary constructor for services where appropriate).

- Styling: do not create `.css` files for components. All styles for a component must be placed in a sibling SCSS file named after the component, e.g. `MyComponent.razor.scss`.

- Use TnTComponents where applicable: prefer the `TnTComponents` library for UI elements and consistent styling. If you are unsure how to use a TnT component, consult the TnTComponents documentation or ask for guidance.

- Accessibility and responsiveness: components should be accessible (use semantic HTML and ARIA where needed) and responsive by default.

- Unit tests: every component must have a corresponding unit test. Use `bUnit` for Blazor component tests and place tests alongside existing test projects following repository conventions.

- Public API: keep the component public surface small. Prefer parameters marked with `[Parameter]` only for values the parent truly needs to control.

- Events and Callbacks: expose actions as `EventCallback`/`EventCallback<T>` to allow consumers to react to component interactions.

- Avoid global state: components should not rely on hidden global state where possible. Pass data and callbacks explicitly via parameters or injected services.

- No regions: do not create `#region` blocks in code-behind files.

- Documentation: add XML documentation to public members on the code-behind class and include a short usage note or example in the component's README or inline comments if the behavior is non-trivial.

- Tests and CI: ensure new or changed component tests run and pass locally with `dotnet test` before submitting changes. Follow existing test patterns using `xUnit`, `NSubstitute`, `AutoFixture`, and `AwesomeAssertions` where relevant.

If any rule is unclear, or you are unsure about how to implement something (for example, TnTComponents usage or testing strategy), ask for clarification before proceeding.

Purpose
 - Give an AI (or developer) a short, deterministic checklist for authoring, reviewing, and testing Blazor components in this repository.

Scope
 - Applies to all UI components implemented as `*.razor` files and their supporting files (code-behind, styles, tests).

Rules (quick checklist)
 1. Single responsibility
    - Each component should do one thing and be small. If it grows, split it into child components.

 2. No inline C# in `.razor`
    - Never use `@code` blocks. Put all logic in a code-behind file named `MyComponent.razor.cs`.

 3. Dependency injection
    - Do not use `@inject` in `.razor`. Use constructor injection in the code-behind class.

 4. Styling
    - Do not create `.css` files. Component styles must live in `MyComponent.razor.scss` placed alongside the component.

 5. TnTComponents
    - Prefer `TnTComponents` for UI elements when appropriate. Consult TnTComponents docs if unsure and ask questions when needed.

 6. Accessibility and responsiveness
    - Use semantic HTML and ARIA attributes where appropriate. Components should behave well on different screen sizes.

 7. Public API and parameters
    - Keep the public surface small. Only expose `[Parameter]` properties that parents must control. Use `EventCallback`/`EventCallback<T>` for events.

 8. Avoid hidden global state
    - Pass state through parameters or explicitly injected services. Do not rely on implicit global variables.

 9. No regions
    - Do not use `#region` blocks in code-behind files.

10. Documentation
    - Add XML docs to public members in code-behind. Include short usage notes or examples in inline comments or component README if behavior is non-trivial.

11. Unit tests
    - Every component must have a unit test using `bUnit`. Place tests in the appropriate test project and follow existing test patterns (xUnit, NSubstitute, AutoFixture, AwesomeAssertions).

12. CI and local verification
    - Run `dotnet test` locally and ensure tests pass before submitting changes.

Guidance for AI reviewers and editors
 - When asked to create or modify a component, follow the checklist above exactly.
 - If any dependency, naming, folder, or framework detail is ambiguous, ask a single clarifying question before making edits.
 - Make minimal, focused changes per PR. Prefer many small commits over large sweeping ones.

Required file layout example
 - `Components/MyWidget/MyWidget.razor` (markup only)
 - `Components/MyWidget/MyWidget.razor.cs` (logic, constructor DI)
 - `Components/MyWidget/MyWidget.razor.scss` (styles)
 - Tests in `Intersect.UnitTests` or other designated test projects using `bUnit`

If anything here conflicts with a higher-priority instruction in repository policy or a reviewer comment, defer to the reviewer and ask for clarification.

Follow these rules for all razor filesthe solution to ensure consistent, maintainable, and high-quality code.