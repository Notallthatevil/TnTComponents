using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NTComponents.Form;

namespace NTComponents.Tests.Form;

/// <summary>
///     Unit tests for <see cref="TnTForm" />.
/// </summary>
public class TnTForm_Tests : BunitContext {

    public TnTForm_Tests() {
        // Default renderer info for tests
        SetRendererInfo(new RendererInfo("WebAssembly", true));
    }

    private class TestModel {
        public string? Name { get; set; }
    }

    private class ChildComponent : ComponentBase {
        [CascadingParameter]
        public ITnTForm? Form { get; set; }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "id", "child-content");
            builder.AddContent(2, "Child Content");
            builder.CloseElement();
        }
    }

    [Fact]
    public void WhenInitialized_WithInteractiveRenderer_AndNoNoValidateAttribute_ThenAddsNoValidateAttribute() {
        // Arrange
        var model = new TestModel();

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, (context) => builder => { })
        );

        // Assert
        var form = cut.Find("form");
        form.HasAttribute("novalidate").Should().BeTrue();
    }

    [Fact]
    public void WhenInitialized_WithInteractiveRenderer_AndNoValidateAttributePresent_ThenPreservesExistingNoValidateAttribute() {
        // Arrange
        SetRendererInfo(new RendererInfo("WebAssembly", true));
        var model = new TestModel();
        var additionalAttributes = new Dictionary<string, object> { { "novalidate", "preserved" } };

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.AdditionalAttributes, additionalAttributes)
            .Add(p => p.ChildContent, (context) => builder => { })
        );

        // Assert
        var form = cut.Find("form");
        form.GetAttribute("novalidate").Should().Be("preserved");
    }

    [Fact]
    public void WhenInitialized_WithStaticRenderer_ThenDoesNotAddNoValidateAttribute() {
        // Arrange
        SetRendererInfo(new RendererInfo("Static", false));
        var model = new TestModel();

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, (context) => builder => { })
        );

        // Assert
        var form = cut.Find("form");
        form.HasAttribute("novalidate").Should().BeFalse();
    }

    [Fact]
    public void WhenRendered_ThenCascadesITnTForm() {
        // Arrange
        var model = new TestModel();

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, (context) => builder => {
                builder.OpenComponent<ChildComponent>(0);
                builder.CloseComponent();
            })
        );

        var capturedForm = cut.FindComponent<ChildComponent>().Instance.Form;

        // Assert
        capturedForm.Should().NotBeNull();
        capturedForm.Should().Be(cut.Instance);
    }

    [Fact]
    public void WhenRendered_ThenRendersChildContent() {
        // Arrange
        var model = new TestModel();

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, (context) => builder => {
                builder.OpenElement(0, "p");
                builder.AddAttribute(1, "id", "test-child");
                builder.AddContent(2, "Child Content");
                builder.CloseElement();
            })
        );

        // Assert
        cut.Find("#test-child").MarkupMatches("<p id='test-child'>Child Content</p>");
    }

    [Fact]
    public void WhenParametersSet_ThenReflectsCorrectState() {
        // Arrange
        var model = new TestModel();

        // Act
        var cut = Render<TnTForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.Appearance, FormAppearance.FilledCompact)
            .Add(p => p.Disabled, true)
            .Add(p => p.ReadOnly, true)
            .Add(p => p.ChildContent, (context) => builder => { })
        );

        // Assert
        cut.Instance.Appearance.Should().Be(FormAppearance.FilledCompact);
        cut.Instance.Disabled.Should().BeTrue();
        cut.Instance.ReadOnly.Should().BeTrue();
    }
}
