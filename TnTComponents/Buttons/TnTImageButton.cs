using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTImageButton : TnTButton {
    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

}