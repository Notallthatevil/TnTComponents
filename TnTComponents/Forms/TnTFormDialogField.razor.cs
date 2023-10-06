using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Common.Ext;

namespace TnTComponents.Forms;
public abstract partial class TnTFormDialogField<TInputType> {

    [Parameter]
    public bool AllowClear { get; set; } = true;

    protected ElementReference BoxInputReference { get; set; }
    protected ElementReference InputElementReference { get; set; }
    protected bool HasSpaceBelow { get; private set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    private DotNetObjectReference<TnTFormDialogField<TInputType>>? _reference;
    private bool _disposedValue;

    protected async Task ClearSelection() {
        if (!Disable) {
            await Deactivate();
            Value = default;
            await OnChange(new ChangeEventArgs() { Value = Value });
        }
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

    protected async Task Focus() {
        if (!Disable) {
            await JSRuntime.SetElementFocus(InputElementReference);
            await OnFocusAsync();
        }
    }
}
