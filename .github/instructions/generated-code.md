## TnTComponents Code Generation Rules

### 1. Component Design Principles
- Each component should do one thing well. Keep components focused, simple, and easy to test.
- They should exhibit a high level of cohesion.
- They should not mix abstraction levels.
- They don't depend on "infrastructure", they depend on abstractions.

- All components must be placed in their own folder under `TnTComponents/`.
- Each component must include: `.razor`, `.razor.cs`, `.razor.scss` (for styles), and optional `.js` if needed.
- If a component includes a `.js` file, it should always define `onLoad`, `onUpdate`, and `onDispose` methods for Blazor interop, following the pattern in [`TnTTabView.razor.js`](../../TnTComponents/TabView/TnTTabView.razor.js).
- All components must inherit from `TnTComponentBase` unless otherwise specified.
- All components should be in the TnTComponents namespace unless otherwise specified.

### 3. Parameters & Events
- Follow established patterns in existing components. If unclear, ask for clarification.

### 4. Styling
- All styles must be written in the `.razor.scss` file using SCSS syntax.
- Use variables from the `_Variables` folder for consistency.
- Colors and other potential parameters should be added to the component's `style` attribute and can then be referenced in the SCSS file for dynamic styling.
- For an example of this pattern, see [`TnTButton.razor.cs`](../../TnTComponents/Buttons/TnTButton.razor.cs) and [`TnTButton.razor.scss`](../../TnTComponents/Buttons/TnTButton.razor.scss).

### 5. Testing
- Each component must have a corresponding test covering functionality and basic rendering.
- Tests should focus on behavior, not markup or style details.

### 6. Documentation
- All generated components must include XML comments and an example code section.

### 7. Theming
- Support theming as best as possible using the project's theming APIs and patterns.
- For an example of themeing, see [`TnTButton.razor.cs`](../../TnTComponents/Buttons/TnTButton.razor.cs) for its color usage and [`TnTButton.razor.scss`](../../TnTComponents/Buttons/TnTButton.razor.scss) for how colors are implemented.

### 8. Accessibility
- Implement accessibility (a11y) best practices: ARIA roles, keyboard navigation, and semantic HTML.

### 9. External Dependencies
- Do not add new third-party packages unless explicitly approved.
