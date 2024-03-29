using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Ext;

namespace TnTComponents.Core;
public abstract class TnTPageScriptComponent<TComponent> : TnTComponentBase, ITnTPageScriptComponent<TComponent>, IAsyncDisposable where TComponent : ComponentBase {
    public abstract string? JsModulePath { get; }
    public IJSObjectReference? IsolatedJsModule { get; private set; }
    public DotNetObjectReference<TComponent>? DotNetObjectRef { get; set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        var derived = this as TComponent;
        DotNetObjectRef = DotNetObjectReference.Create(derived!);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            IsolatedJsModule = await JSRuntime.ImportIsolatedJs(this, JsModulePath);
            await (IsolatedJsModule?.InvokeVoidAsync("onLoad", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }

        await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
    }

    public async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }

    protected RenderFragment RenderPageScript() {
        return new RenderFragment(builder => {
            builder.OpenComponent<TnTPageScript>(0);
            builder.AddAttribute(1, "Src", JsModulePath);
            builder.CloseComponent();
        });
    }
}

