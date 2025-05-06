using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a chip component with various customizable properties and events.
/// </summary>
public partial class TnTChip : TnTComponentBase, ITnTInteractable {

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    /// <summary>
    ///     The event callback to be invoked after the value is bound.
    /// </summary>
    [Parameter]
    public EventCallback<bool> BindAfter { get; set; }

    /// <summary>
    ///     The event callback to be invoked when the close button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> CloseButtonClicked { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the toggle functionality is disabled.
    /// </summary>
    [Parameter]
    public bool DisableToggle { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
                            .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTInteractable(this)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddClass("tnt-togglable", !DisableToggle)
        .AddClass("tnt-chip")
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <summary>
    ///     The label of the chip.
    /// </summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; } = TnTColor.OnPrimary;

    /// <summary>
    ///     Gets or sets a value indicating whether the chip is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     The start icon of the chip.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <summary>
    ///     The text color of the chip.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

    /// <summary>
    ///     The value of the chip.
    /// </summary>
    public bool Value { get; set; }

    /// <summary>
    ///     The event callback to be invoked when the value changes.
    /// </summary>
    public EventCallback<bool> ValueChanged { get; set; }

    /// <summary>
    ///     The cascading parameter for the form.
    /// </summary>
    [CascadingParameter]
    private ITnTForm? _form { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "id", ElementId);

        if (StartIcon is not null) {
            builder.AddContent(50, StartIcon.Render());
        }

        builder.AddContent(60, Label);

        builder.OpenElement(70, "input");
        builder.AddAttribute(80, "type", "checkbox");
        builder.AddAttribute(90, "class", "tnt-chip-checkbox");
        builder.AddAttribute(100, "disabled", _form?.Disabled ?? Disabled);
        builder.AddAttribute(110, "readonly", _form?.ReadOnly ?? ReadOnly);
        builder.AddAttribute(120, "title", ElementTitle);
        builder.AddAttribute(130, "lang", ElementLang);
        builder.AddAttribute(140, "value", bool.TrueString);
        if (!DisableToggle) {
            builder.AddAttribute(150, "checked", BindConverter.FormatValue(Value));
            builder.AddAttribute(160, "onchange", EventCallback.Factory.CreateBinder(this, value => { Value = value; BindAfter.InvokeAsync(Value); }, Value));
            builder.SetUpdatesAttributeName("checked");
        }
        builder.CloseElement();

        if (CloseButtonClicked.HasDelegate) {
            builder.OpenComponent<TnTImageButton>(170);
            builder.AddComponentParameter(180, nameof(TnTImageButton.Icon), MaterialIcon.Close);
            builder.AddComponentParameter(190, nameof(TnTImageButton.OnClickCallback), CloseButtonClicked);
            builder.AddComponentParameter(200, nameof(TnTImageButton.BackgroundColor), TnTColor.Transparent);
            builder.AddComponentParameter(210, nameof(TnTImageButton.TextColor), TextColor);
            builder.AddComponentParameter(220, nameof(TnTImageButton.TintColor), TintColor);
            builder.AddComponentParameter(230, nameof(TnTImageButton.Elevation), 0);
            builder.AddComponentParameter(240, nameof(TnTImageButton.StopPropagation), true);
            builder.AddComponentParameter(250, "class", "tnt-chip-close-button");
            builder.CloseComponent();
        }

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(260);
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}