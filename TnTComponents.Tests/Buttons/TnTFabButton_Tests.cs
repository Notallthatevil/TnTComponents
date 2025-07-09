using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.Buttons;

public class TnTFabButton_Tests : Bunit.TestContext
{
    private const string RippleJsModule = "./_content/TnTComponents/Core/TnTRippleEffect.razor.js";

    public TnTFabButton_Tests()
    {
        var rippleModule = JSInterop.SetupModule(RippleJsModule);
        rippleModule.SetupVoid("onLoad", _ => true);
    }

    [Fact]
    public void Renders_FabButton_And_ChildContent()
    {
        // Act
        var cut = RenderComponent<TnTFabButton>(parameters => parameters
            .AddChildContent("<span>FAB</span>")
        );

        // Assert: container div exists
        var container = cut.Find(".tnt-fab-button-container");
        Assert.NotNull(container);
        // Assert: button exists with correct class
        var button = cut.Find("button.tnt-fab-button");
        Assert.NotNull(button);
        // Assert: child content is rendered
        Assert.Contains("FAB", button.InnerHtml);
    }

    [Fact]
    public void Renders_RippleEffect_When_Enabled()
    {
        var cut = RenderComponent<TnTFabButton>(parameters => parameters
            .Add(p => p.EnableRipple, true)
        );
        // Assert: ripple effect is present
        var ripple = cut.Find("tnt-ripple-effect");
        Assert.NotNull(ripple);
    }

    [Fact]
    public void Does_Not_Render_RippleEffect_When_Disabled()
    {
        var cut = RenderComponent<TnTFabButton>(parameters => parameters
            .Add(p => p.EnableRipple, false)
        );
        // Assert: ripple effect is not present
        Assert.Empty(cut.FindAll("tnt-ripple-effect"));
    }

    [Fact]
    public void Button_Is_Disabled_When_Prop_Set()
    {
        var cut = RenderComponent<TnTFabButton>(parameters => parameters
            .Add(p => p.Disabled, true)
        );
        var button = cut.Find("button");
        Assert.True(button.HasAttribute("disabled"));
    }
}
