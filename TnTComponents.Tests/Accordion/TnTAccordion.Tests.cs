using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;
using TnTComponents;
using RippleTestingUtility = TnTComponents.Tests.TestingUtility.TestingUtility;

namespace TnTComponents.Tests.Accordion;

public class TnTAccordion_Tests : BunitContext {
    private const string JsModulePath = "./_content/TnTComponents/Accordion/TnTAccordion.razor.js";

    public TnTAccordion_Tests() {
        Renderer.SetRendererInfo(new RendererInfo("Server", isInteractive: true));
        var module = JSInterop.SetupModule(JsModulePath);
        module.SetupVoid("onLoad", _ => true).SetVoidResult();
        module.SetupVoid("onUpdate", _ => true).SetVoidResult();
        module.SetupVoid("onDispose", _ => true).SetVoidResult();
        RippleTestingUtility.SetupRippleEffectModule(this);
        JSInterop.SetupVoid("TnTComponents.addHidden", _ => true).SetVoidResult();
    }

    private static RenderFragment Children(params Action<RenderTreeBuilder>[] children) => b => { foreach (var c in children) c(b); };
    private static Action<RenderTreeBuilder> Child(Action<RenderTreeBuilder> a) => b => { b.OpenComponent<TnTAccordionChild>(0); a(b); b.CloseComponent(); };

    [Fact]
    public void Renders_Accordion_Base_Class() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(Child(b => b.AddAttribute(1, "Label", "A")))));
        // Assert
        cut.Markup.Should().Contain("tnt-accordion");
    }

    [Fact]
    public void Adds_LimitOneExpanded_Class_When_Enabled() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.Add(c => c.LimitToOneExpanded, true).AddChildContent(Children(Child(b => b.AddAttribute(1, "Label", "A")))));
        // Assert
        cut.Markup.Should().Contain("tnt-limit-one-expanded");
    }

    [Fact]
    public void Renders_Child_Labels() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => b.AddAttribute(1, "Label", "First")),
            Child(b => b.AddAttribute(1, "Label", "Second")))));
        // Assert
        cut.Markup.Should().Contain("First");
    }

    [Fact]
    public void Renders_Second_Child_Label() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => b.AddAttribute(1, "Label", "First")),
            Child(b => b.AddAttribute(1, "Label", "Second")))));
        // Assert
        cut.Markup.Should().Contain("Second");
    }

    [Fact]
    public void Orders_Children_By_Order_Property() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "Second"); b.AddAttribute(2, "Order", 20); }),
            Child(b => { b.AddAttribute(1, "Label", "First"); b.AddAttribute(2, "Order", 10); })
        )));
        // Assert
        var firstIndex = cut.Markup.IndexOf("First", StringComparison.Ordinal);
        var secondIndex = cut.Markup.IndexOf("Second", StringComparison.Ordinal);
        (firstIndex < secondIndex).Should().BeTrue();
    }

    [Fact]
    public void Multiple_OpenByDefault_When_Not_Limited_All_Expanded() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "OpenByDefault", true); }),
            Child(b => { b.AddAttribute(1, "Label", "B"); b.AddAttribute(2, "OpenByDefault", true); })
        )));
        // Assert
        cut.FindAll(".tnt-expanded").Count.Should().Be(2);
    }

    [Fact]
    public void Multiple_OpenByDefault_When_Limited_Only_First_Expanded() {
        // Arrange
        // Act
        var cut = Render<TnTAccordion>(p => p.Add(c => c.LimitToOneExpanded, true)
            .AddChildContent(Children(
                Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "OpenByDefault", true); }),
                Child(b => { b.AddAttribute(1, "Label", "B"); b.AddAttribute(2, "OpenByDefault", true); })
            )));
        // Assert
        cut.FindAll(".tnt-expanded").Count.Should().Be(1);
    }

    [Fact]
    public async Task SetAsOpened_Invokes_OnOpenCallback_When_Closed() {
        // Arrange
        var opened = 0;
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, nameof(TnTAccordionChild.OnOpenCallback), EventCallback.Factory.Create(this, () => opened++)); }))));
        var child = cut.FindComponents<TnTAccordionChild>().Single().Instance;
        var elementId = (int)typeof(TnTAccordionChild).GetField("_elementId", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child)!;
        // Act
        await cut.Instance.SetAsOpened(elementId);
        cut.Render();
        // Assert
        opened.Should().Be(1);
    }

    [Fact]
    public async Task SetAsClosed_Invokes_OnCloseCallback_When_Open() {
        // Arrange
        var closed = 0;
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "OpenByDefault", true); b.AddAttribute(3, nameof(TnTAccordionChild.OnCloseCallback), EventCallback.Factory.Create(this, () => closed++)); }))));
        var child = cut.FindComponents<TnTAccordionChild>().Single().Instance;
        var elementId = (int)typeof(TnTAccordionChild).GetField("_elementId", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child)!;
        // Act
        await cut.Instance.SetAsClosed(elementId);
        cut.Render();
        // Assert
        closed.Should().Be(1);
    }

    [Fact]
    public async Task LimitToOneExpanded_CloseAllAsync_Invoked_On_Second_Open() {
        // Arrange
        var cut = Render<TnTAccordion>(p => p.Add(c => c.LimitToOneExpanded, true)
            .AddChildContent(Children(
                Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "OpenByDefault", true); }),
                Child(b => { b.AddAttribute(1, "Label", "B"); })
            )));
        var second = cut.FindComponents<TnTAccordionChild>().Last().Instance;
        var secondId = (int)typeof(TnTAccordionChild).GetField("_elementId", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(second)!;
        var initial = JSInterop.Invocations.Count(i => i.Identifier == "TnTComponents.addHidden");
        // Act
        await cut.Instance.SetAsOpened(secondId);
        cut.Render();
        // Assert
        JSInterop.Invocations.Count(i => i.Identifier == "TnTComponents.addHidden").Should().BeGreaterThan(initial);
    }

    [Fact]
    public async Task SetAsOpened_Does_Not_Invoke_OnOpen_When_Already_Open() {
        // Arrange
        var opened = 0;
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "OpenByDefault", true); b.AddAttribute(3, nameof(TnTAccordionChild.OnOpenCallback), EventCallback.Factory.Create(this, () => opened++)); }))));
        var child = cut.FindComponents<TnTAccordionChild>().Single().Instance;
        var elementId = (int)typeof(TnTAccordionChild).GetField("_elementId", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child)!;
        // Act
        await cut.Instance.SetAsOpened(elementId);
        cut.Render();
        // Assert
        opened.Should().Be(0);
    }

    [Fact]
    public async Task SetAsClosed_Does_Not_Invoke_OnClose_When_Already_Closed() {
        // Arrange
        var closed = 0;
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, nameof(TnTAccordionChild.OnCloseCallback), EventCallback.Factory.Create(this, () => closed++)); }))));
        var child = cut.FindComponents<TnTAccordionChild>().Single().Instance;
        var elementId = (int)typeof(TnTAccordionChild).GetField("_elementId", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child)!;
        // Act
        await cut.Instance.SetAsClosed(elementId);
        cut.Render();
        // Assert
        closed.Should().Be(0);
    }

    [Fact]
    public async Task Child_Dispose_Removes_From_Dom() {
        // Arrange
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => b.AddAttribute(1, "Label", "First")),
            Child(b => b.AddAttribute(1, "Label", "Second"))
        )));
        var second = cut.FindComponents<TnTAccordionChild>().Last();
        // Act
        await cut.InvokeAsync(() => second.Instance.Dispose());
        cut.Render();
        // Assert
        cut.Markup.Should().NotContain("Second");
    }

    [Fact]
    public void Child_EnableRipple_False_Renders_No_Ripple_Component() {
        // Arrange / Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "EnableRipple", false); }))));
        // Assert
        cut.Markup.Should().NotContain("tnt-ripple-effect");
    }

    [Fact]
    public void Child_ContentStyle_Includes_Custom_Color_Variable() {
        // Arrange / Act
        var cut = Render<TnTAccordion>(p => p.AddChildContent(Children(
            Child(b => { b.AddAttribute(1, "Label", "A"); b.AddAttribute(2, "ContentBodyColor", TnTColor.Primary); }))));
        // Assert
        cut.Find(".tnt-accordion-child").GetAttribute("style")!.Should().Contain("--tnt-accordion-child-content-bg-color");
    }
}
