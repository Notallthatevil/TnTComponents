using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Xunit;
using TnTComponents;
using TnTComponents.Tests.TestingUtility;

namespace TnTComponents.Tests.Editors;

/// <summary>
///     Unit tests for <see cref="TnTMarkdownEditor" />.
/// </summary>
public class TnTMarkdownEditor_Tests : BunitContext {

    public TnTMarkdownEditor_Tests() {
        var module = JSInterop.SetupModule("./_content/TnTComponents/Editors/TnTMarkdownEditor.razor.js");
        module.SetupVoid();
    }

    [Fact]
    public void Renders_Basic_MarkdownEditor_Structure() {
        // Act
        var cut = Render<TnTMarkdownEditor>();
        var root = cut.Find("tnt-markdown-editor");
        var textarea = cut.Find("textarea");

        // Assert
        root.Should().NotBeNull();
        textarea.Should().NotBeNull();
        textarea.GetAttribute("style").Should().Contain("display:none");
    }

    [Fact]
    public void Has_TnTId_Attribute() {
        // Act
        var cut = Render<TnTMarkdownEditor>();
        var root = cut.Find("tnt-markdown-editor");

        // Assert
        root.HasAttribute("tntid").Should().BeTrue();
        root.GetAttribute("tntid").Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ElementClass_Returns_Empty_String() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.ElementClass.Should().Be(string.Empty);
    }

    [Fact]
    public void ElementStyle_Returns_Empty_String() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.ElementStyle.Should().Be(string.Empty);
    }

    [Fact]
    public void JsModulePath_Returns_Correct_Path() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.JsModulePath.Should().Be("./_content/TnTComponents/Editors/TnTMarkdownEditor.razor.js");
    }

    [Fact]
    public void ElementId_Is_Set_On_Initialization() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.ElementId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Renders_InitialValue_When_Provided() {
        // Arrange
        var initialValue = "# Hello World\n\nThis is **bold** text.";

        // Act
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.InitialValue, initialValue));
        var initialValueDiv = cut.Find("div.initial-value");

        // Assert
        initialValueDiv.Should().NotBeNull();
        initialValueDiv.TextContent.Should().Contain("# Hello World");
        initialValueDiv.TextContent.Should().Contain("This is **bold** text.");
        initialValueDiv.GetAttribute("style").Should().Contain("display:none");
    }

    [Fact]
    public void Does_Not_Render_InitialValue_When_Null_Or_Empty() {
        // Act
        var cut = Render<TnTMarkdownEditor>();
        var initialValueDivs = cut.FindAll("div.initial-value");

        // Assert
        initialValueDivs.Should().BeEmpty();
    }

    [Fact]
    public void Does_Not_Render_InitialValue_When_Whitespace_Only() {
        // Act
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.InitialValue, "   \n\t  "));
        var initialValueDivs = cut.FindAll("div.initial-value");

        // Assert
        initialValueDivs.Should().BeEmpty();
    }

    [Fact]
    public void Value_Parameter_Is_Bindable() {
        // Arrange
        var value = "# Test Content";

        // Act
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.Value, value));

        // Assert
        cut.Instance.Value.Should().Be(value);
    }

    [Fact]
    public void RenderedHtml_Parameter_Is_Bindable() {
        // Arrange
        var renderedHtml = new MarkupString("<h1>Test Content</h1>");

        // Act
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.RenderedHtml, renderedHtml));

        // Assert
        cut.Instance.RenderedHtml.Should().Be(renderedHtml);
    }

    [Fact]
    public async Task UpdateValue_Updates_Value_And_Fires_ValueChanged() {
        // Arrange
        var valueChanged = false;
        var newValue = string.Empty;
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<string>(this, (v) => {
                valueChanged = true;
                newValue = v;
            })));

        // Act
        await cut.Instance.UpdateValue("# New Content", "<body><h1>New Content</h1></body>");

        // Assert
        cut.Instance.Value.Should().Be("# New Content");
        valueChanged.Should().BeTrue();
        newValue.Should().Be("# New Content");
    }

    [Fact]
    public async Task UpdateValue_Updates_RenderedHtml_And_Fires_RenderedHtmlChanged() {
        // Arrange
        var renderedHtmlChanged = false;
        var newRenderedHtml = new MarkupString();
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.RenderedHtmlChanged, EventCallback.Factory.Create<MarkupString?>(this, (v) => {
                renderedHtmlChanged = true;
                newRenderedHtml = v ?? new MarkupString();
            })));

        // Act
        await cut.Instance.UpdateValue("# New Content", "<body><h1>New Content</h1></body>");

        // Assert
        cut.Instance.RenderedHtml.Should().NotBeNull();
        cut.Instance.RenderedHtml!.Value.Value.Should().Be("<h1>New Content</h1>");
        renderedHtmlChanged.Should().BeTrue();
        newRenderedHtml.Value.Should().Be("<h1>New Content</h1>");
    }

    [Fact]
    public async Task UpdateValue_Extracts_Body_Content_From_Html() {
        // Arrange
        var cut = Render<TnTMarkdownEditor>();

        // Act
        await cut.Instance.UpdateValue("# Test", "<html><head></head><body><h1>Test</h1><p>Content</p></body></html>");

        // Assert
        cut.Instance.RenderedHtml!.Value.Value.Should().Be("<h1>Test</h1><p>Content</p>");
    }

    [Fact]
    public async Task UpdateValue_Handles_Html_Without_Body_Tag() {
        // Arrange
        var cut = Render<TnTMarkdownEditor>();

        // Act
        await cut.Instance.UpdateValue("# Test", "<h1>Test</h1><p>Content</p>");

        // Assert
        // When no body tag is found, the regex should not match and RenderedHtml should not be set
        cut.Instance.RenderedHtml.Should().BeNull();
    }

    [Fact]
    public async Task UpdateValue_Handles_Empty_Body() {
        // Arrange
        var cut = Render<TnTMarkdownEditor>();

        // Act
        await cut.Instance.UpdateValue("", "<body></body>");

        // Assert
        cut.Instance.Value.Should().Be("");
        cut.Instance.RenderedHtml!.Value.Value.Should().Be("");
    }

    [Fact]
    public void Applies_Custom_AdditionalAttributes() {
        // Arrange
        var attrs = new Dictionary<string, object>
        {
            { "class", "custom-class" },
            { "data-testid", "markdown-editor" }
        };

        // Act
        var cut = Render<TnTMarkdownEditor>(p => p
            .Add(x => x.AdditionalAttributes, attrs));
        var root = cut.Find("tnt-markdown-editor");

        // Assert
        root.GetAttribute("class").Should().Contain("custom-class");
        root.GetAttribute("data-testid").Should().Be("markdown-editor");
    }

    [Fact]
    public void Inherits_From_TnTPageScriptComponent() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.Should().BeAssignableTo<TnTComponents.Core.TnTPageScriptComponent<TnTMarkdownEditor>>();
    }

    [Fact]
    public void PageScript_RenderFragment_Is_Included() {
        // Act
        var cut = Render<TnTMarkdownEditor>();
        var pageScript = cut.FindAll("tnt-page-script");

        // Assert
        pageScript.Should().NotBeEmpty();
        pageScript.First().GetAttribute("src").Should().Be("./_content/TnTComponents/Editors/TnTMarkdownEditor.razor.js");
    }

    [Fact]
    public void JSInvokable_UpdateValue_Can_Be_Called_From_JavaScript() {
        // Arrange
        var cut = Render<TnTMarkdownEditor>();
        var instance = cut.Instance;

        // Act - Simulate JavaScript calling the JSInvokable method
        var method = typeof(TnTMarkdownEditor).GetMethod(nameof(TnTMarkdownEditor.UpdateValue));
        var jsInvokableAttr = method?.GetCustomAttributes(typeof(JSInvokableAttribute), false).FirstOrDefault();

        // Assert
        jsInvokableAttr.Should().NotBeNull();
        jsInvokableAttr.Should().BeOfType<JSInvokableAttribute>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n\t")]
    [InlineData(null)]
    public async Task UpdateValue_Handles_Empty_Or_Null_Values(string? value) {
        // Arrange
        var cut = Render<TnTMarkdownEditor>();

        // Act
        await cut.Instance.UpdateValue(value ?? string.Empty, "<body></body>");

        // Assert
        cut.Instance.Value.Should().Be(value ?? string.Empty);
    }

    [Fact]
    public void ComponentIdentifier_Is_Unique_Across_Instances() {
        // Act
        var cut1 = Render<TnTMarkdownEditor>();
        var cut2 = Render<TnTMarkdownEditor>();

        // Assert
        cut1.Instance.ComponentIdentifier.Should().NotBe(cut2.Instance.ComponentIdentifier);
        cut1.Instance.ComponentIdentifier.Should().NotBeNullOrEmpty();
        cut2.Instance.ComponentIdentifier.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Component_Implements_IAsyncDisposable() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.Should().BeAssignableTo<IAsyncDisposable>();
    }

    [Fact]
    public void Component_Implements_IDisposable() {
        // Act
        var cut = Render<TnTMarkdownEditor>();

        // Assert
        cut.Instance.Should().BeAssignableTo<IDisposable>();
    }
}