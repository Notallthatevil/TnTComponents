using AwesomeAssertions;

namespace NTComponents.Tests.Buttons.NTButtonGroup;

/// <summary>
///     Verifies the rendering behavior of the <see cref="NTButtonGroup{TObjectType}" /> component.
/// </summary>
public sealed class Render_Tests : NTButtonGroupTestContext {
    /// <summary>
    ///     Ensures the component renders one button for each provided item.
    /// </summary>
    [Fact]
    public void WithMultipleItems_RendersAButtonForEachItem() {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = Render<NTButtonGroup<string>>(parameters => parameters.AddChildContent(RenderItems(items)));
        var buttons = cut.FindAll("button.btn-group-btn");

        // Assert
        buttons.Count.Should().Be(items.Count);
    }

    /// <summary>
    ///     Validates that the connected display type emits the correct CSS modifier.
    /// </summary>
    [Fact]
    public void WithConnectedDisplayType_AddsConnectedModifier() {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = Render<NTButtonGroup<string>>(parameters => parameters
            .AddChildContent(RenderItems(items))
            .Add(p => p.DisplayType, NTButtonGroupDisplayType.Connected));
        var container = cut.Find("div.nt-button-group");

        // Assert
        var classes = container.GetAttribute("class")!;
        classes.Should().Contain("nt-button-group--connected");
        classes.Should().NotContain("nt-button-group--disconnected");
    }

    /// <summary>
    ///     Ensures icon-only items render the image button variant.
    /// </summary>
    [Fact]
    public void WithIconOnlyItem_RendersImageButton() {
        // Arrange
        var items = CreateItems(iconOnlyFirstItem: true);

        // Act
        var cut = Render<NTButtonGroup<string>>(parameters => parameters.AddChildContent(RenderItems(items)));
        var imageButton = cut.Find("button.tnt-image-button");

        // Assert
        imageButton.Should().NotBeNull();
    }
}
