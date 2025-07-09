using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.Buttons;

public class TnTImageButton_Tests : Bunit.TestContext
{
    private const string RippleJsModule = "./_content/TnTComponents/Core/TnTRippleEffect.razor.js";

    public TnTImageButton_Tests()
    {
        var rippleModule = JSInterop.SetupModule(RippleJsModule);
        rippleModule.SetupVoid("onLoad", _ => true);
    }


    // Minimal concrete subclass for TnTIcon
    private class TestIcon : TnTComponents.TnTIcon
    {
        public TestIcon()
        {
            Icon = "test-icon";
        }
        public override string? ElementClass => null;
        public override string? ElementStyle => null;
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "i");
            builder.AddContent(1, Icon);
            builder.CloseElement();
        }
    }

    [Fact]
    public void Renders_ImageButton_And_Icon()
    {
        var icon = new TestIcon();
        var cut = RenderComponent<TnTImageButton>(parameters => parameters
            .Add(p => p.Icon, icon)
        );
        var button = cut.Find("button.tnt-image-button");
        Assert.NotNull(button);
        Assert.Contains("test-icon", button.InnerHtml);
    }

    [Fact]
    public void Renders_Badge_When_Provided()
    {
        var icon = new TestIcon();
        var badge = TnTComponents.TnTBadge.CreateBadge("1");
        var cut = RenderComponent<TnTImageButton>(parameters => parameters
            .Add(p => p.Icon, icon)
            .Add(p => p.Badge, badge)
        );
        Assert.Contains("1", cut.Markup);
    }

    [Fact]
    public void Renders_RippleEffect_When_Enabled()
    {
        var icon = new TestIcon();
        var cut = RenderComponent<TnTImageButton>(parameters => parameters
            .Add(p => p.Icon, icon)
            .Add(p => p.EnableRipple, true)
        );
        var ripple = cut.Find("tnt-ripple-effect");
        Assert.NotNull(ripple);
    }

    [Fact]
    public void Does_Not_Render_RippleEffect_When_Disabled()
    {
        var icon = new TestIcon();
        var cut = RenderComponent<TnTImageButton>(parameters => parameters
            .Add(p => p.Icon, icon)
            .Add(p => p.EnableRipple, false)
        );
        Assert.Empty(cut.FindAll("tnt-ripple-effect"));
    }

    [Fact]
    public void Button_Is_Disabled_When_Prop_Set()
    {
        var icon = new TestIcon();
        var cut = RenderComponent<TnTImageButton>(parameters => parameters
            .Add(p => p.Icon, icon)
            .Add(p => p.Disabled, true)
        );
        var button = cut.Find("button");
        Assert.True(button.HasAttribute("disabled"));
    }
}
