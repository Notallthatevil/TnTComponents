using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.Badge;

public class TnTBadge_Tests : Bunit.TestContext
{
    [Fact]
    public void Renders_Badge_With_Content()
    {
        // Act
        var cut = RenderComponent<TnTBadge>(parameters => parameters
            .AddChildContent("BadgeContent")
        );
        // Assert: badge element exists
        var badge = cut.Find(".tnt-badge");
        Assert.NotNull(badge);
        // Assert: content is rendered
        Assert.Contains("BadgeContent", badge.InnerHtml);
    }

    [Fact]
    public void Applies_BackgroundColor_And_TextColor()
    {
        var cut = RenderComponent<TnTBadge>(parameters => parameters
            .Add(p => p.BackgroundColor, TnTColor.Primary)
            .Add(p => p.TextColor, TnTColor.OnPrimary)
            .AddChildContent("ColorTest")
        );
        var badge = cut.Find(".tnt-badge");
        var style = badge.GetAttribute("style");
        Assert.Contains("tnt-badge-background-color", style);
        Assert.Contains("tnt-badge-text-color", style);
    }

    [Fact]
    public void Applies_AdditionalAttributes()
    {
        var cut = RenderComponent<TnTBadge>(parameters => parameters
            .AddUnmatched("data-test", "value")
            .AddChildContent("AttrTest")
        );
        var badge = cut.Find(".tnt-badge");
        Assert.Equal("value", badge.GetAttribute("data-test"));
    }
}
