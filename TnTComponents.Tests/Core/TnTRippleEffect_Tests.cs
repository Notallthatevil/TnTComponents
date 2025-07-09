using Bunit;
using Xunit;
using TnTComponents.Core;

namespace TnTComponents.Tests.Core;

public class TnTRippleEffect_Tests : Bunit.TestContext
{
    private const string JsModulePath = "./_content/TnTComponents/Core/TnTRippleEffect.razor.js";

    public TnTRippleEffect_Tests()
    {
        var module = JSInterop.SetupModule(JsModulePath);
        module.SetupVoid("onLoad", _ => true);
    }

    [Fact]
    public void Renders_RippleEffect_Element()
    {
        // Act
        var cut = RenderComponent<TnTRippleEffect>();

        // Assert: tnt-ripple-effect element exists
        var ripple = cut.Find("tnt-ripple-effect");
        Assert.NotNull(ripple);
        // Assert: tnt-page-script is present with correct src
        var script = cut.Find("tnt-page-script");
        Assert.Equal(JsModulePath, script.GetAttribute("src"));
    }
}
