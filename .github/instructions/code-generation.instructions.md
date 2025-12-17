---
applyTo: "**/*.cs"
---

# Code Generation Guidelines

## Documentation
- **Public API:** All public types and members must have XML documentation (`/// <summary>`) describing intent and behavior.
- **Internal Code:** Document internal types if behavior is complex or business-critical.
- **Focus:** Explain "why" and "what", not implementation details.

## Testability
- **Design:** Write small, single-responsibility methods. Avoid monolithic functions.
- **Coverage:** Add unit tests for all new behavior. Follow the repository's test layout rules.

## Abstractions & Interfaces
- **YAGNI:** Do NOT create interfaces solely for mocking. Test concrete behavior where possible.
- **Justification:** If introducing an abstraction, explain:
  1. The problem being solved.
  2. Why concrete implementation is insufficient.
  3. Expected lifecycle and alternatives.
- **Visibility:** Prefer `internal` and `sealed` for non-extensible implementations.

## Design & Style
- **Conventions:** Follow existing naming and folder layout.
- **Surface Area:** Keep public API minimal.
- **Dependency Injection:** Use DI for cross-cutting concerns or host-provided services.

## Project Structure
- **Placement:** Add code to the most appropriate existing project (Core, Data, etc.).
- **New Projects:** Do NOT create new top-level projects without explicit justification.
- **Dependencies:** Add NuGet packages to the smallest scope possible.

## Security
- **Secrets:** **NEVER** embed secrets, credentials, or environment-specific values in code. Use configuration/secret stores.

## AI Workflow
- **Markers:** If fully auto-generated, add a header comment indicating the tool/script.
- **Review:** Summarize what was generated and why. Ensure CI passes.
