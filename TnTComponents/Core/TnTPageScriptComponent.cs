using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Ext;

namespace TnTComponents.Core;

public abstract class TnTPageScriptComponent<TComponent> : TnTComponentBase, ITnTPageScriptComponent<TComponent>, IAsyncDisposable where TComponent : ComponentBase {
    public DotNetObjectReference<TComponent>? DotNetObjectRef { get; set; }
    public IJSObjectReference? IsolatedJsModule { get; private set; }
    public abstract string? JsModulePath { get; }

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    protected RenderFragment PageScript;

    protected TnTPageScriptComponent() {
        PageScript = new RenderFragment(builder => {
            builder.OpenComponent<TnTPageScript>(0);
            builder.AddAttribute(1, "Src", JsModulePath);
            builder.CloseComponent();
        });
    }

    public virtual async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }

            DotNetObjectRef?.Dispose();
        }
        catch (JSDisconnectedException) { }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        try {
            if (firstRender) {
                IsolatedJsModule = await JSRuntime.ImportIsolatedJs(this, JsModulePath);
                await (IsolatedJsModule?.InvokeVoidAsync("onLoad", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
            }

            await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }
        catch (JSDisconnectedException) { }
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        var derived = this as TComponent;
        DotNetObjectRef = DotNetObjectReference.Create(derived!);
    }
}