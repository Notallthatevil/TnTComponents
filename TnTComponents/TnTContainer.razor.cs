using Microsoft.AspNetCore.Components;

namespace TnTComponents;

public partial class TnTContainer {

    [Parameter]
    public override string Class { get; set; } = "tnt-container";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
}