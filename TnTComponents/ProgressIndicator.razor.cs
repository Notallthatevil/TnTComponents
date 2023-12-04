using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class ProgressIndicator {

    [Parameter]
    public ProgressAppearance Appearance { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public override string? Class { get; set; } = "tnt-progress-indicator";

    [Parameter]
    public double Max { get; set; }

    [Parameter]
    public double? Value { get; set; }

    public override string GetClass() => base.GetClass() + " " + Appearance.ToString().ToLower();
}