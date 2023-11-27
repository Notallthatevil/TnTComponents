using Microsoft.AspNetCore.Components;

namespace TnTComponents;

public partial class TnTSegmentedButton {
    public object? ActiveObject { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public override string? Class { get; set; } = "tnt-segmented-button";
}