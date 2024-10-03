using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTChip : TnTInteractableComponentBase {

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
    public TnTIcon? StartIcon { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    [Parameter]
    public override TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;
    [Parameter]
    public override TnTColor? OnTintColor { get; set; } = TnTColor.OnPrimary;

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

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddAttribute(10, "class", ElementClass);
        builder.AddAttribute(20, "style", ElementStyle);
        builder.AddAttribute(30, "id", ElementId);
        builder.AddMultipleAttributes(40, AdditionalAttributes);

        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start-icon";
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
            builder.AddComponentParameter(180, nameof(TnTImageButton.Icon), new MaterialIcon { Icon = MaterialIcon.Close });
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

    //[Parameter]
    //public TnTColor ActiveColor { get; set; } = TnTColor.Primary;

    //[Parameter]
    //public TnTColor ActiveTextColor { get; set; } = TnTColor.OnPrimary;

    //[Parameter]
    //public TnTBorderRadius? BorderRadius { get; set; } = new TnTBorderRadius(2);

    //[Parameter]
    //public RenderFragment ChildContent { get; set; } = default!;

    //public override string? ElementClass => CssClassBuilder.Create()
    //    .AddFromAdditionalAttributes(AdditionalAttributes)
    //    .AddClass("tnt-chip")
    //    .AddBorderRadius(BorderRadius)
    //    .Build();

    //public override string? ElementStyle => CssStyleBuilder.Create()
    //    .AddFromAdditionalAttributes(AdditionalAttributes)
    //    .AddVariable("active-color", $"var(--tnt-color-{ActiveColor.ToCssClassName()})")
    //    .AddVariable("active-text-color", $"var(--tnt-color-{ActiveTextColor.ToCssClassName()})")
    //    .AddVariable("inactive-text-color", $"var(--tnt-color-{InactiveTextColor.ToCssClassName()})")
    //    .Build();

    //[Parameter]
    //public TnTColor InactiveTextColor { get; set; } = TnTColor.Primary;

    //[Parameter]
    //public TnTIcon? StartIcon { get; set; }

    //[Parameter]
    //public bool Value { get; set; }

    //[Parameter]
    //public EventCallback<bool> ValueChanged { get; set; }

    ////protected override string? JsModulePath => "./_content/TnTComponents/Chips/TnTChip.razor.js";

    //protected override void OnParametersSet() {
    //    base.OnParametersSet();
    //    if (StartIcon is not null) {
    //        StartIcon.AdditionalClass = "tnt-start";
    //    }
    //}

}