using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTChip {

    [Parameter]
    public TnTColor ActiveColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTColor ActiveTextColor { get; set; } = TnTColor.OnPrimary;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new TnTBorderRadius(2);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-chip")
        .AddBorderRadius(BorderRadius)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("active-color", $"var(--tnt-color-{ActiveColor.ToCssClassName()})")
        .AddVariable("active-text-color", $"var(--tnt-color-{ActiveTextColor.ToCssClassName()})")
        .AddVariable("inactive-text-color", $"var(--tnt-color-{InactiveTextColor.ToCssClassName()})")
        .Build();

    [Parameter]
    public TnTColor InactiveTextColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    //protected override string? JsModulePath => "./_content/TnTComponents/Chips/TnTChip.razor.js";

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start";
        }
    }
}