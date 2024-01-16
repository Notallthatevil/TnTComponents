using Microsoft.AspNetCore.Components;

namespace TnTComponents
public partial class TnTForm {
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }
}
