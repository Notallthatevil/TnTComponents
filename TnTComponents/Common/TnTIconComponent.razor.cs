using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents.Common;
public abstract partial class TnTIconComponent {
    [Parameter]
    public virtual string Icon { get; set; } = default!;

    [Parameter]
    public IconType IconType { get; set; }
}
