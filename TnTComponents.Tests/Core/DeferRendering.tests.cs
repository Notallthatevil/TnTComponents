using Bunit;
using Xunit;
using TnTComponents.Core;
using Microsoft.AspNetCore.Components;
using AwesomeAssertions;

namespace TnTComponents.Tests.Core;

public class DeferRenderingTests : IClassFixture<Bunit.TestContext>
{
    private readonly Bunit.TestContext _testContext;

    public DeferRenderingTests(Bunit.TestContext testContext)
    {
        _testContext = testContext;
    }

    [Fact]
    public void RendersChildContent_WhenProvided()
    {
        // Arrange
        var content = (RenderFragment)(builder => builder.AddContent(0, "Hello World"));

        // Act
        var cut = _testContext.RenderComponent<DeferRendering>(parameters => parameters
            .Add(p => p.ChildContent, content));

        // Assert
        cut.Markup.Should().Contain("Hello World");
    }

    [Fact]
    public void RendersNothing_WhenChildContentIsNull()
    {
        // Act
        var cut = _testContext.RenderComponent<DeferRendering>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment?)null));

        // Assert
        cut.Markup.Should().BeEmpty();
    }
}
