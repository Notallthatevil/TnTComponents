using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a divider component with customizable color, margin, and orientation.
/// </summary>
public class TnTDivider : TnTComponentBase {

    /// <summary>
    ///     Gets or sets the color of the divider.
    /// </summary>
    [Parameter]
    public TnTColor? Color { get; set; } = TnTColor.OutlineVariant;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .AddClass($"tnt-divider-{Orientation.ToString().ToLower()}")
            .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddStyle("margin-top", $"{Margin}px", Margin is not null && Orientation == Orientation.Horizontal)
            .AddStyle("margin-bottom", $"{Margin}px", Margin is not null && Orientation == Orientation.Horizontal)
            .AddStyle("margin-left", $"{Margin}px", Margin is not null && Orientation == Orientation.Vertical)
            .AddStyle("margin-right", $"{Margin}px", Margin is not null && Orientation == Orientation.Vertical)
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .AddVariable("divider-color", $"var(--tnt-color-{Color.ToCssClassName()})", Color.HasValue)
            .Build();

    /// <summary>
    ///     Gets or sets the margin of the divider in pixels.
    /// </summary>
    [Parameter]
    public int? Margin { get; set; } = 8;

    /// <summary>
    ///     Gets or sets the orientation of the divider.
    /// </summary>
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "title", ElementTitle);
        builder.AddAttribute(50, "lang", ElementLang);
        builder.AddAttribute(60, "id", ElementId);
        builder.CloseElement();
    }
}