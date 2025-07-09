using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;
using TnTComponents.Core;

namespace TnTComponents.Tests.Core;

public class TnTExternalClickHandler_Tests : Bunit.TestContext
{
    private const string JsModulePath = "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";

    public TnTExternalClickHandler_Tests()
    {
        // Mock the JS module methods
        var module = JSInterop.SetupModule(JsModulePath);
        module.SetupVoid("externalClickCallbackRegister", _ => true);
        module.SetupVoid("externalClickCallbackDeregister", _ => true);
        // Mock the onLoad call used by TnTPageScriptComponent (accept any arguments)
        module.SetupVoid("onLoad", _ => true);
    }

    [Fact]
    public void Renders_ChildContent()
    {
        // Act
        var cut = RenderComponent<TnTExternalClickHandler>(parameters => parameters
            .Add(p => p.ExternalClickCallback, EventCallback.Factory.Create(this, () => { }))
            .AddChildContent("<span>Test Content</span>")
        );

        // Assert: div has correct classes
        var div = cut.Find("div");
        var classAttr = div.GetAttribute("class");
        Assert.Contains("tnt-external-click-handler", classAttr);
        Assert.Contains("tnt-components", classAttr);
        // Assert: tntid attribute exists
        Assert.True(div.HasAttribute("tntid"));
        // Assert: child content is rendered
        Assert.Contains("Test Content", div.InnerHtml);
        // Assert: tnt-page-script is present with correct src
        var script = cut.Find("tnt-page-script");
        Assert.Equal("./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js", script.GetAttribute("src"));
    }

    [Fact]
    public async Task Invokes_ExternalClickCallback_OnClick()
    {
        bool callbackInvoked = false;
        var cut = RenderComponent<TnTExternalClickHandler>(parameters => parameters
            .Add(p => p.ExternalClickCallback, EventCallback.Factory.Create(this, () => callbackInvoked = true))
            .AddChildContent("<span>Test</span>")
        );

        // Simulate JSInvokable call
        await cut.Instance.OnClick();

        Assert.True(callbackInvoked);
    }
}
