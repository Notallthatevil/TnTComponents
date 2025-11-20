---
applyTo: "**/*.Tests.cs"
---

Testing frameworks and guidelines

## IMPORTANT
You must use Microsoft Testing Platform syntax when testing [MTP docs](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-mtp)

---
Frameworks
- Mocking: `NSubstitute` — use for creating test doubles and verifying interactions.
- Test data: `AutoFixture` — use to generate input data and minimize manual setup; prefer customizing fixtures when specific values are required.
- Test runner/framework: `xUnit.v3` — use as the test framework for all test projects.
- Assertions: `AwesomeAssertions` — use for fluent, descriptive assertions that improve readability.

Test style and quality
- Tests must be concise and focused. Each test should assert a single behavior or outcome.
- Use the `Arrange-Act-Assert` (AAA) pattern and include explicit comments for each section:
  - Arrange: set up inputs and mocks
  - Act: execute the code under test
  - Assert: verify results
- Common test setup can be done in the test class constructor; teardown should be implemented via `IDisposable` and the `Dispose` method to clean up resources.
- Naming conventions:
  - Test class names: Because each production method gets its own test class/file, name the test class after the production method and suffix it with `_Tests` (for example: `CalculatePay_Tests`).
  - Test method names: Since the test class already identifies the production method under test, test method names can omit the production method name and focus on the behavior or state under test, using an intent-based convention such as `StateUnderTest_ExpectedBehavior` (for example: `WithOvertime_ReturnsCorrectAmount`).
  - Alternative full-name convention (not allowed): `MethodName_StateUnderTest_ExpectedBehavior`, they should always be named `StateUnderTest_ExpectedBehavior`
- Prefer `AutoFixture` to create generic data; override specific values explicitly in the `Arrange` step when behavior depends on them.
- Use `NSubstitute` to verify interactions and to stub external dependencies. Keep verifications explicit and minimal.
- Use `AwesomeAssertions` for clear, fluent assertions; prefer expressive checks over primitive boolean asserts.
- Documentation is not required but welcome. Inline comments are also helpful but not required. 
- Explicit Arrange, Act, Assert comments are required.

Test project layout and file organization
- Each production class under test should have its own folder inside the corresponding test project. Use the production class name with a `Tests` suffix or mirror the production folder structure.
- Within the class folder, create a separate `.cs` file for each method under test. File names should be `MethodName.Tests.cs`. The test class inside the file should be named `MethodName_Tests`. (Replace the `.Tests` with `_Tests`)
- Example layout:
  - `AccountingPayroll.Core.Tests/Domain/Employee_Tests/CalculatePay.Tests.cs`

Coverage and test generation
- Aim for high code coverage for critical code paths and all edge cases. Generate tests for boundary conditions, error handling, and uncommon inputs.
- When creating tests automatically (e.g., via generators or templates), ensure tests reflect the intent of the code — they must assert business intent, not just implementation details.
- Tests should include edge cases and negative scenarios. If a code path appears untested or fragile, add tests that demonstrate the potential failure modes.

Use of Testcontainers for integration tests
- For tests that require a real database or external service, prefer `Testcontainers for .NET` to provide ephemeral, isolated Docker-backed containers.
- Keep Testcontainers-based tests separate from unit tests and mark them as integration tests (e.g., by using an `Integration` category or separate project). Do not run container-backed tests by default in fast unit test runs.
- CI must support Docker to run these tests. Document any CI changes required to enable Docker-based integration tests.
- Ensure containers are started and disposed cleanly between test runs to avoid resource leaks and flaky tests.
- Do not embed secrets in test code. Use test-friendly credentials or CI-provided secrets when necessary.

Bug reporting
- When writing or generating tests, always point out any bugs or unexpected behavior discovered in the code under test. Include a short description of the observed issue and, if possible, a suggested fix or area to investigate.

Other rules
- Keep tests fast and isolated. Avoid external dependencies (databases, networks) in unit tests; use integration tests for end-to-end scenarios.
- Do not commit secrets or environment-specific configuration to test projects.
- Update or add tests whenever modifying business logic. Tests are part of the change and should accompany code fixes or features.

Follow these rules for all test projects under the solution to ensure consistent, maintainable, and high-quality tests.