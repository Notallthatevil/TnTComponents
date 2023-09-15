using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace TnTComponents.Button;

public abstract partial class TnTButtonBase {
    [Parameter]
    public string? Icon { get; set; }
    [Parameter]
    public string? IconClass { get; set; }
    [Parameter]
    public string? Text { get; set; }
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected abstract string GetClass();
}
