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
- All component tests should be in a `.cs` file. 
- Unit tests **DO NOT** need to be documented.
- Avoid duplicated code within test class
- Only verify markup that belongs to the component being tested.
- Stick to one logical assertion per test.
- Create custom assertion methods using business language, if needed.  
- Use Arrange, Act, Assert pattern.
- Attempt to write tests as black box tests. Use the public API of the component only and documentation to determine behavior.

## Running Tests

- Run all tests with `dotnet test` from the solution root or the test project directory.
- Code coverage is automatically collected if supported by the test runner.

## Test Rules
- Tests should be comprehensive, provide a high code coverage, and cover edge cases. 
- Tests should be written with the intent of the code in mind, not necessarily after the code that is written. This will help discover flaws in the code. 

## File Names
- Test file names should be include `.Tests.(whatever the file extension is)` i.e. `SomeClass.Tests.cs` or `SomeComponent.Tests.razor`. 
- For class names, they should replace the `.Tests` with `_Tests` i.e. `SomeClass_Tests`.

## References

- See `TnTComponents.Tests.csproj` for package and project references.
