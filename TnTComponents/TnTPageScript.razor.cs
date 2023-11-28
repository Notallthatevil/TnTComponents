using Microsoft.AspNetCore.Components;

namespace TnTComponents;
public partial class TnTPageScript
{
    [Parameter, EditorRequired]
    public string Src { get; set; } = default!;

}
