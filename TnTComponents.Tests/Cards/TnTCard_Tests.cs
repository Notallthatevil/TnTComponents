using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.Cards;

public class TnTCard_Tests : Bunit.TestContext
{
    [Fact]
    public void Renders_Card_With_ChildContent()
    {
        // Act
        var cut = RenderComponent<TnTCard>(parameters => parameters
            .AddChildContent("CardContent")
        );
        // Assert: card element exists
        var card = cut.Find(".tnt-card");
        Assert.NotNull(card);
        // Assert: content is rendered
        Assert.Contains("CardContent", card.InnerHtml);
    }

    [Fact]
    public void Applies_Appearance_Classes()
    {
        var filled = RenderComponent<TnTCard>(parameters => parameters
            .Add(p => p.Appearance, CardAppearance.Filled)
        );
        Assert.Contains("tnt-card-filled", filled.Find(".tnt-card").GetAttribute("class"));

        var outlined = RenderComponent<TnTCard>(parameters => parameters
            .Add(p => p.Appearance, CardAppearance.Outlined)
        );
        Assert.Contains("tnt-card-outlined", outlined.Find(".tnt-card").GetAttribute("class"));

        var elevated = RenderComponent<TnTCard>(parameters => parameters
            .Add(p => p.Appearance, CardAppearance.Elevated)
        );
        Assert.Contains("tnt-card-elevated", elevated.Find(".tnt-card").GetAttribute("class"));
    }

    [Fact]
    public void Applies_BackgroundColor_And_TextColor()
    {
        var cut = RenderComponent<TnTCard>(parameters => parameters
            .Add(p => p.BackgroundColor, TnTColor.Primary)
            .Add(p => p.TextColor, TnTColor.OnPrimary)
            .AddChildContent("ColorTest")
        );
        var card = cut.Find(".tnt-card");
        var style = card.GetAttribute("style");
        Assert.Contains("tnt-card-background-color", style);
        Assert.Contains("tnt-card-text-color", style);
    }

    [Fact]
    public void Applies_AdditionalAttributes()
    {
        var cut = RenderComponent<TnTCard>(parameters => parameters
            .AddUnmatched("data-test", "value")
            .AddChildContent("AttrTest")
        );
        var card = cut.Find(".tnt-card");
        Assert.Equal("value", card.GetAttribute("data-test"));
    }
}
