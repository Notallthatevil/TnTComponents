using Bunit;
using Xunit;
using TnTComponents.Core;
using Microsoft.AspNetCore.Components;
using AwesomeAssertions;

namespace TnTComponents.Tests.Core;

public class TnTExternalClickHandlerTests : IClassFixture<Bunit.TestContext>
{
    private readonly Bunit.TestContext _testContext;

    public TnTExternalClickHandlerTests(Bunit.TestContext testContext)
    {
        _testContext = testContext;
    }

    [Fact]
    public void RendersChildContent()
    {
        // Arrange
        var content = (RenderFragment)(builder => builder.AddContent(0, "Click Me!"));
        var callback = EventCallback.Factory.Create(this, () => { });

        // Act
        var cut = _testContext.RenderComponent<TnTExternalClickHandler>(parameters => parameters
            .Add(p => p.ChildContent, content)
            .Add(p => p.ExternalClickCallback, callback));

        // Assert
        cut.Markup.Should().Contain("Click Me!");
    }

    [Fact]
    public async Task InvokesExternalClickCallback_OnClick()
    {
        // Arrange
        bool callbackInvoked = false;
        var callback = EventCallback.Factory.Create(this, () => callbackInvoked = true);
        var cut = _testContext.RenderComponent<TnTExternalClickHandler>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Test")))
            .Add(p => p.ExternalClickCallback, callback));

        // Act
        await cut.Instance.OnClick();

        // Assert
        callbackInvoked.Should().BeTrue();
    }
}
