---
applyTo: "**/*.razor.css,**/*.razor.scss"
---

Purpose
- Describe how styles should be authored, organized, and applied across the repository. These rules are specifically written so an automated assistant or developer can deterministically create or update styles.

Scope
- Applies to all component-level styles (`*.razor.scss`) and any shared/global SCSS partials used by the web projects in this workspace.

Core rules
1. SCSS only
   - Do not create `.css` files for components. Always create a `*.razor.scss` file next to the component markup (`MyComponent.razor`).
   - If a build pipeline requires a compiled `.css`, that pipeline is responsible for compiling SCSS artifacts into CSS — do not check in compiled CSS.

2. File placement and naming
   - Component styles must be a sibling file named `MyComponent.razor.scss` and scoped to the component by prefixing selectors with a predictable root class (see "scoping" below).
   - Shared styles, variables, and mixins must live in a central `styles` or `scss` folder (e.g. `ClientApp/styles/_variables.scss`, `ClientApp/styles/_mixins.scss`). Use partials (leading `_`) and import them where needed.

3. Scoping and selectors
   - Each component must use a single root class to scope its styles. Name it after the component, e.g. `.my-component` for `MyComponent.razor`.
   - Keep selectors shallow (max 3 levels deep) and avoid high-specificity selectors. Prefer classes over element selectors.
   - Use a lightweight BEM-like pattern for modifiers and elements (e.g. `.my-component__title`, `.my-component--small`).

4. TnTComponents and utilities
   - Prefer using TnTComponents or repository-provided utility classes for spacing, layout and common patterns when available.
   - If you must implement a UI pattern not covered by TnTComponents, keep styles local to the component and consider contributing a reusable utility or partial.

5. Responsiveness and accessibility
   - Ensure styles support common responsive breakpoints and do not hide content from assistive technologies.
   - Avoid using `display:none` to hide information important to accessibility; use ARIA attributes and proper semantics instead.

6. Avoid globals and !important
   - Avoid global selectors or modifying element defaults in component SCSS. Use global partials only for truly global concerns (reset, typography, theme tokens).
   - Do not use `!important` except in rare, justified cases with an inline comment explaining why.

7. Documentation and comments
   - Add a short header comment in each `*.razor.scss` describing the component and any assumptions (e.g. required markup structure or modifier classes).

8. Linting and formatting
   - Follow the repository's SCSS linting and formatting rules if present. Keep consistent indentation and organization (variables, imports, base rules, modifiers).

AI-specific guidance
- When creating or updating styles, follow this sequence:
  1. Create or update `MyComponent.razor.scss` alongside the `.razor` file. Do not modify or create compiled `.css` files.
  2. At the top of the SCSS import shared tokens: `@use "../../ClientApp/styles/variables" as *;` or the repository's canonical path — if unknown, ask one question to locate the variables file.
  3. Add a single root class matching the component name (kebab-case). Scope all rules under that root.
  4. Use variables and shared mixins for colors, spacing, and breakpoints.
  5. Keep rules small and focused. If more than ~120 lines are needed, consider splitting styles or refactoring the component.
  6. Include a short header comment describing why styles exist and any external expectations (e.g., required parent layout or accessibility notes).

Verification
- After making style changes, verify visually by running the application locally or by asking the user to run and confirm visual changes. If automated visual tests exist, ensure they pass.

If you are unsure about conventions (variables location, build pipeline for SCSS), ask one clarifying question before making edits.