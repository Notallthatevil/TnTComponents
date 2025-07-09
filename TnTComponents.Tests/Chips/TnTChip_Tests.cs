using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.Chips
{
    public class TnTChip_Tests : Bunit.TestContext
    {
        public TnTChip_Tests()
        {
            var module = this.JSInterop.SetupModule("./_content/TnTComponents/Core/TnTRippleEffect.razor.js");
            module.SetupVoid("onLoad", _ => true);
        }

    [Fact]
    public void Renders_Chip_With_Label()
    {
        // Act
        var cut = RenderComponent<TnTChip>(parameters => parameters
            .Add(p => p.Label, "ChipLabel")
        );
        // Assert: chip element exists
        var chip = cut.Find(".tnt-chip");
        Assert.NotNull(chip);
        // Assert: label is rendered
        Assert.Contains("ChipLabel", chip.InnerHtml);
    }

    [Fact]
    public void Applies_BackgroundColor_And_TextColor()
    {
        var cut = RenderComponent<TnTChip>(parameters => parameters
            .Add(p => p.Label, "ColorTest")
            .Add(p => p.BackgroundColor, TnTColor.Primary)
            .Add(p => p.TextColor, TnTColor.OnPrimary)
        );
        var chip = cut.Find(".tnt-chip");
        var style = chip.GetAttribute("style");
        Assert.Contains("tnt-chip-background-color", style);
        Assert.Contains("tnt-chip-text-color", style);
    }

    [Fact]
    public void Applies_AdditionalAttributes()
    {
        var cut = RenderComponent<TnTChip>(parameters => parameters
            .Add(p => p.Label, "AttrTest")
            .AddUnmatched("data-test", "value")
        );
        var chip = cut.Find(".tnt-chip");
        Assert.Equal("value", chip.GetAttribute("data-test"));
    }
}
}
