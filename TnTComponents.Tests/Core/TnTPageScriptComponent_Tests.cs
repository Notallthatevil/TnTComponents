using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;
using Xunit;

namespace TnTComponents.Tests.Core;

public class TnTPageScriptComponent_Tests : Bunit.TestContext {

    [Fact]
    public void Constructor_ThrowsIfTypeMismatch() {
        Assert.Throws<InvalidCastException>(() => new InvalidDerived());
    }

    [Fact]
    public async Task DisposeAsyncCore_DoesNotThrow_WhenNoJsModule() {
        var component = new TestScriptComponent();
        await component.DisposeAsync();
        component.DotNetObjectRef.Should().BeNull();
        component.IsolatedJsModule.Should().BeNull();
    }

    [Fact]
    public void PageScript_RendersCorrectly() {
        // Arrange
        var module = JSInterop.SetupModule("_content/TestModule.js");
        module.SetupVoid();
        var cut = RenderComponent<TestScriptComponent>();

        // Act
        var fragment = cut.Instance.PageScript;

        // Assert
        var rendered = Render(fragment);
        rendered.MarkupMatches("<tnt-page-script src=\"_content/TestModule.js\"></tnt-page-script>");
    }

    private class InvalidDerived : TnTPageScriptComponent<TestScriptComponent> {
        public override string? ElementClass => null;
        public override string? ElementStyle => null;
        public override string? JsModulePath => null;
    }

    private class TestScriptComponent : TnTPageScriptComponent<TestScriptComponent> {

        // Implement required abstract members from TnTComponentBase
        public override string? ElementClass => null;

        public override string? ElementStyle => null;
        public override string? JsModulePath => "_content/TestModule.js";
        public new RenderFragment PageScript => base.PageScript;
    }
}