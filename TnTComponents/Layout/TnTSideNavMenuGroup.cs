using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
/// A collapsible navigation group component for side navigation menus.
/// </summary>
/// <remarks>
/// Provides a toggleable group of navigation items with an expandable/collapsible section.
/// </remarks>
public class TnTSideNavMenuGroup : TnTComponentBase, ITnTInteractable, ITnTStyleable {
    /// <summary>
    /// Gets or sets a value indicating whether the group is expanded by default.
    /// </summary>
    /// <value>
    /// <c>true</c> if the group should be expanded when initially rendered; otherwise, <c>false</c>.
    /// Default is <c>true</c>.
    /// </value>
    [Parameter]
    public bool ExpandByDefault { get; set; } = true;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-side-nav-menu-group")
        .AddClass("tnt-toggle", ExpandByDefault)
        .AddDisabled(Disabled)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle { get; }

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; }
    
    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;
    
    /// <inheritdoc />
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
    
    /// <inheritdoc />
    [Parameter]
    public int Elevation { get; set; }
    
    /// <inheritdoc />
    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);
    
    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    /// <summary>
    /// Gets or sets the content of the navigation group.
    /// </summary>
    /// <value>
    /// The child elements to be rendered inside the navigation group.
    /// </value>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the icon displayed alongside the label.
    /// </summary>
    /// <value>
    /// The icon to display, or <c>null</c> if no icon should be shown.
    /// </value>
    [Parameter]
    public TnTIcon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the text label for the navigation group.
    /// </summary>
    /// <value>
    /// The text to display for the navigation group header.
    /// </value>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    /// Builds the component's render tree.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to populate with content.</param>
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(80, "autofocus", AutoFocus);
        builder.AddElementReferenceCapture(90, e => Element = e);

        builder.OpenElement(100, "div");
        builder.AddAttribute(110, "class", CssClassBuilder.Create()
            .AddClass("tnt-side-nav-menu-group-button")
            .AddTnTInteractable(this)
            .AddTnTStyleable(this)
            .AddFilled()
            .Build()
        );
        builder.AddAttribute(120, "onclick", "TnTComponents.toggleSideNavGroup(event)");

        builder.OpenElement(121, "span");
        if (Icon is not null) {
            builder.AddContent(130, Icon.Render());
        }

        builder.AddContent(140, Label);
        builder.CloseElement();

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(145);
            builder.CloseComponent();
        }


        builder.OpenComponent<MaterialIcon>(150);
        builder.AddComponentParameter(160, nameof(MaterialIcon.Icon), MaterialIcon.ArrowDropDown.Icon);
        builder.AddAttribute(161, "class", "tnt-end-icon");
        builder.CloseComponent();

        builder.CloseElement();

        // Data permanent section
        {
            builder.OpenElement(170, "div");
            builder.AddAttribute(180, "class", "tnt-side-nav-group-toggle-indicator");
            builder.AddAttribute(190, "style", "display:none");
            builder.AddAttribute(200, "data-permanent");

            builder.OpenElement(210, "div");
            builder.AddAttribute(220, "class", $"tnt-toggle-indicator{(ExpandByDefault ? " tnt-toggle" : string.Empty)}");
            builder.CloseElement();

            builder.CloseElement();
        }

        builder.AddContent(230, ChildContent);

        builder.CloseElement();
    }
}