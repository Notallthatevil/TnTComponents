# TnTComponents Unit Test Instructions

## Test Frameworks & Tools

- Use **xUnit v3** for all unit tests (`xunit.v3`, `xunit.runner.visualstudio`).
- Use **bUnit** for Blazor component testing.
- Use **AwesomeAssertions** for fluent assertions in tests.
- Use **NSubstitute** for mocking.
- Use **AutoFixture** for any 
- Code coverage is collected using **coverlet.collector**.

## Project Setup

- Target framework is **.NET 9.0**.
- Tests are in the `TnTComponents.Tests` project, which references the main `TnTComponents` library.
- Implicit usings and nullable reference types are enabled.
- Output type is `Exe` (test runner compatible).

## Test Organization

- Organize tests by component, mirroring the structure of the main library.
- Place tests in folders named after the component (e.g., `Buttons/`, `Accordion/`).

## Writing Tests

- Use `[Fact]` and `[Theory]` attributes from xUnit for test methods.
- Use bUnit's `TestContext` for rendering and interacting with Blazor components.
- Use AwesomeAssertions for expressive, readable assertions.
- Focus tests on component functionality and behavior, not markup or style details.
- Each component should have tests for basic rendering and key behaviors.
- Always inherit from `Bunit.TestContext` explicity. 
- All component tests should be in a `.razor` file and be within the `@code` block. 
- Unit tests **DO NOT** need to be documented.
- Use bUnit's `Render` function with inline razor markup instead of `RenderComponent`
- Avoid duplicated code within test class
- Only verify markup that belongs to the component being tested.
- Stick to one logical assertion per test.
- Create custom assertion methods using business language, if needed.  

## Running Tests

- Run all tests with `dotnet test` from the solution root or the test project directory.
- Code coverage is automatically collected if supported by the test runner.

## References

- See `TnTComponents.Tests.csproj` for package and project references.
