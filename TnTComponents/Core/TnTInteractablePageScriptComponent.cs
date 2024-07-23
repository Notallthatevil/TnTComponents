using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Ext;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;
public abstract class TnTInteractablePageScriptComponent<TComponent> : TnTComponentBase, ITnTPageScriptComponent<TComponent>, ITnTInteractable, IAsyncDisposable where TComponent : ComponentBase {
    [Parameter]
    // <inheritdoc/>
    public bool Disabled { get; set; }

    [Parameter]
    public string? Name { get; set; }

    public DotNetObjectReference<TComponent>? DotNetObjectRef { get; set; }
    public IJSObjectReference? IsolatedJsModule { get; private set; }
    public abstract string? JsModulePath { get; }

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;
    [Parameter]
    public bool EnableRipple { get; set; } = true;
    [Parameter]
    public virtual TnTColor? TintColor { get; set; }

    protected RenderFragment PageScript;

    public virtual async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            IsolatedJsModule = await JSRuntime.ImportIsolatedJs(this, JsModulePath);
            await (IsolatedJsModule?.InvokeVoidAsync("onLoad", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }

        await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        var derived = this as TComponent;
        DotNetObjectRef = DotNetObjectReference.Create(derived!);
    }
}