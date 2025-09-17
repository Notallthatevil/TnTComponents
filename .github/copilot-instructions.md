
## TnTComponents – Copilot Quick Guide

### 0. Prime Directive
Never assume. Mirror existing patterns. If something is ambiguous, pause and surface the question instead of guessing.

### 1. Core Principles
* Small, cohesive, testable components (Material 3 look & feel).
* Each component lives in its own folder under `TnTComponents/`.
* Favor consistency over novelty; reuse existing abstractions.
* No new external packages without explicit approval.

### 2. Creating / Extending a Component
Required files (per folder):
* `Name.razor` (markup only – logic goes in code-behind)
* `Name.razor.cs` (partial class inheriting `TnTComponentBase` unless clearly excluded)
* `Name.razor.scss` (author styles here) → compiled to `.razor.css` / `.razor.min.css`
* Optional `Name.razor.js` if JS interop is needed; must export: `onLoad`, `onUpdate`, `onDispose` (see `TabView` example)

Patterns:
* Parameters: use `[Parameter]` (and `[EditorRequired]` when truly necessary); follow naming & style from similar existing components (e.g., `TnTButton`).
* Events: expose strongly-typed callbacks (`EventCallback<T>`). Avoid leaking internal state types unless already public.
* Cascading values: mirror existing usage for theme / density / localization if needed.
* Theming: dynamic values exposed via inline `style` variables → referenced in SCSS (see button example). Theme JSON lives in `wwwroot/Themes` and is consumed via the theme toggle component.
* Accessibility: semantic HTML first; add appropriate `role`, `aria-*`, focus order, and keyboard interactions consistent with Material guidance.

### 3. Styling Rules
* Only SCSS in `.razor.scss`; do not hand-edit generated `.css`.
* Reuse shared variables from `_Variables` (never hard-code palette values unless transient/testing).
* Keep selectors component-scoped; avoid global leakage.

### 4. Testing Essentials
Refer to `instructions/unit-tests.md`, but remember:
* One folder in `TnTComponents.Tests/` mirroring component path.
* Component tests in `.Tests.cs` files; inherit from `BunitContext`.
* Use Arrange / Act / Assert; one logical behavioral assertion per test.
* Focus on behavior (rendered effect, events, state transitions), not internal markup structure unless it's the behavior.
* Attempt to achieve 100% coverage.
* Always, always, always refer to `instructions/unit-tests.md` when writing tests.

### 5. Dev Workflows
* Build: `dotnet restore` then `dotnet build` (solution root).
* Test: `dotnet test` (or narrow scope by project / filter).
* Manual verification: run `LiveTest` app (client for UI smoke checks).
* Keep the build green before adding more features—incremental, verified changes.

### 6. Documentation Rules
* XML doc every public type/member. Describe purpose & behavior (avoid “Gets or sets ...”).
* Use `/// <inheritdoc />` for overrides / interface implementations.
* Include a short usage example in new component summaries when practical.

### 7. When Adding JavaScript
* Only if necessary (performance, browser API gaps, complex measurements).
* Namespaced file alongside component.
* Lifecycle exports: `onLoad(element, options)`, `onUpdate(element, options)`, `onDispose(element)`.
* Keep interop surface minimal & typed (consider a C# options record / DTO).

### 8. Do / Don't (Quick Scan)
Do:
* Follow existing naming & folder conventions.
* Add tests with new functionality.
* Keep PR-sized changes cohesive.
* Support keyboard & screen reader access.
Don't:
* Introduce new dependencies silently.
* Embed logic directly in `.razor` unless trivially declarative.
* Duplicate patterns (abstract / extend when repetition emerges).
* Hard-code theme colors where variables exist.

### 9. Reference Maps
* Code gen rules: `./instructions/generated-code.md`
* Testing: `./instructions/unit-tests.md`
* Style guide: `./instructions/code-style.md`
* Examples to consult first: `Buttons/`, `Accordion/`, `TabView/` for JS pattern.

### 10. Escalation Triggers
Surface clarification instead of proceeding when:
* API shape conflicts with an existing analogous component.
* A feature implies cross-cutting concerns (theming, globalization, virtualization).
* Requires altering shared base classes.

---
Concise summary: Mirror existing component structure; keep logic in code-behind; SCSS only for styling; tests mirror structure; document everything; prefer consistency and accessibility; ask when uncertain.