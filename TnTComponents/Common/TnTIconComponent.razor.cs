using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents.Common;
public abstract partial class TnTIconComponent {
    [Parameter]
    public string Icon { get; set; } = default!;

    [Parameter]
    public IconType IconType { get; set; }
}
