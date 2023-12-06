using Microsoft.AspNetCore.Components;

namespace TnTComponents;
public partial class TnTTabContent {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
}
