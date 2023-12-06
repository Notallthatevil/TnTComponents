using Microsoft.AspNetCore.Components;

namespace TnTComponents;
public partial class TnTTabHeader {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
}
