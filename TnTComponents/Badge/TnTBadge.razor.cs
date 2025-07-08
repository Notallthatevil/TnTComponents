using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Security.Cryptography.X509Certificates;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a badge component with customizable properties.
/// </summary>
public partial class TnTBadge {

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Error;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-badge")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-badge-background-color", BackgroundColor)
        .AddVariable("tnt-badge-text-color", TextColor)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; } = TextAlign.Center;

    /// <inheritdoc />
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnError;

    /// <summary>
    ///     Gets the render fragment for the badge.
    /// </summary>
    internal RenderFragment Render() => BuildRenderTree;

    /// <summary>
    ///     Creates a new instance of <see cref="TnTBadge" /> with specified content and properties.
    /// </summary>
    /// <param name="content">        The content to be displayed inside the badge.</param>
    /// <param name="backgroundColor">The background color of the badge.</param>
    /// <param name="textAlignment">  The text alignment within the badge.</param>
    /// <param name="textColor">      The text color of the badge.</param>
    /// <param name="cssClass">       Additional CSS classes for the badge.</param>
    /// <param name="cssStyle">       Additional CSS styles for the badge.</param>
    /// <returns>A new instance of <see cref="TnTBadge" />.</returns>
    public static TnTBadge CreateBadge(string content, TnTColor backgroundColor = TnTColor.Error, TextAlign? textAlignment = TextAlign.Center, TnTColor textColor = TnTColor.OnError, string? cssClass = null, string? cssStyle = null) {
        Dictionary<string, object> additionalAttributes = [];
        if (!string.IsNullOrWhiteSpace(cssClass)) {
            additionalAttributes.Add("class", cssClass);
        }

        if (!string.IsNullOrWhiteSpace(cssStyle)) {
            additionalAttributes.Add("style", cssStyle);
        }
        return new TnTBadge {
            BackgroundColor = backgroundColor,
            ChildContent = new RenderFragment(builder => {
                builder.AddContent(0, content);
            }),
            TextAlignment = textAlignment,
            TextColor = textColor,
            AdditionalAttributes = additionalAttributes
        };
    }
}