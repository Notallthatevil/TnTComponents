using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common.Ext;

namespace TnTComponents.Forms;
public abstract class TnTFormDialogField<TInputType> : TnTFormField<TInputType> {

    protected ElementReference BoxInputReference { get; set; }
    protected ElementReference InputElementReference { get; set; }
    protected bool HasSpaceBelow { get; private set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected async Task ClearSelection() {
        await Deactivate();
        Value = default;
        await OnChange(new ChangeEventArgs() { Value = Value });
    }

    protected async Task Deactivate() {
        await JSRuntime.RemoveElementFocus(InputElementReference);
        Active = false;
    }

    protected async Task OnFocusAsync() {
        var rect = await JSRuntime.GetElementBoundingRectForComponent(BoxInputReference);
        var windowHeight = await JSRuntime.GetWindowHeight();
        if (rect is not null) {
            var distanceToBottom = windowHeight - rect.Bottom;
            HasSpaceBelow = distanceToBottom >= await JSRuntime.ConvertRemToPixels(15);
        }
        Active = true;
    }
}
