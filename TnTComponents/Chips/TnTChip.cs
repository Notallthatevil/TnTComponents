using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public partial class TnTChip : TnTComponentBase, ITnTInteractable {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTInteractable(this)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddClass("tnt-togglable", !DisableToggle)
        .AddClass("tnt-chip")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();


    [Parameter]
    public RenderFragment<TnTIcon>? StartIcon { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;
    [Parameter]
    public TnTColor? OnTintColor { get; set; } = TnTColor.OnPrimary;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public bool DisableToggle { get; set; }

    [CascadingParameter]
    private ITnTForm? _form { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    public bool Value { get; set; }

    public EventCallback<bool> ValueChanged { get; set; }
    [Parameter]
    public EventCallback<bool> BindAfter { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> CloseButtonClicked { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? ElementName { get; set; }

    [Parameter]
    public bool EnableRipple { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "id", ElementId);

        if (StartIcon is not null) {
            builder.AddContent(50, StartIcon);
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
            builder.AddComponentParameter(180, nameof(TnTImageButton.Icon), MaterialIcon.Close );
            builder.AddComponentParameter(190, nameof(TnTImageButton.OnClickCallback), CloseButtonClicked);
            builder.AddComponentParameter(200, nameof(TnTImageButton.BackgroundColor), TnTColor.Transparent);
            builder.AddComponentParameter(210, nameof(TnTImageButton.TextColor), TextColor);
            builder.AddComponentParameter(220, nameof(TnTImageButton.TintColor), TintColor);
            builder.AddComponentParameter(230, nameof(TnTImageButton.Elevation), 0);
            builder.AddComponentParameter(240, nameof(TnTImageButton.StopPropagation), true);
            builder.AddComponentParameter(250, "class", "tnt-chip-close-button");
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}