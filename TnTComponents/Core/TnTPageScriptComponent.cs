using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Ext;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;

/// <summary>
///     Represents a base class for components that have an isolated JavaScript module.
/// </summary>
/// <typeparam name="TComponent">The type of the component.</typeparam>
public abstract class TnTPageScriptComponent<TComponent> : TnTComponentBase, ITnTPageScriptComponent<TComponent> where TComponent : ComponentBase {
    public DotNetObjectReference<TComponent>? DotNetObjectRef { get; set; }

    public IJSObjectReference? IsolatedJsModule { get; private set; }

    public abstract string? JsModulePath { get; }

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    /// <summary>
    ///     Gets the render fragment for the page script.
    /// </summary>
    protected RenderFragment PageScript;

    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TnTPageScriptComponent{TComponent}" /> class.
    /// </summary>
    protected TnTPageScriptComponent() {
        PageScript = new RenderFragment(builder => {
            builder.OpenComponent<TnTPageScript>(0);
            builder.AddAttribute(1, "Src", JsModulePath);
            builder.CloseComponent();
        });
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the component.
    /// </summary>
    /// <param name="disposing">
    ///     A value indicating whether the method is called from the Dispose method.
    /// </param>
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            DotNetObjectRef?.Dispose();
            DotNetObjectRef = null;

            if (IsolatedJsModule is IDisposable disposable) {
                disposable.Dispose();
                IsolatedJsModule = null;
            }
        }

        _disposed = true;
    }

    /// <summary>
    ///     Asynchronously disposes the core resources of the component.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async ValueTask DisposeAsyncCore() {
        if (IsolatedJsModule is not null) {
            try {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync().ConfigureAwait(false);
            }
            catch (JSDisconnectedException) { }
        }

        if (DotNetObjectRef is IAsyncDisposable asyncDisposable) {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else {
            DotNetObjectRef?.Dispose();
        }

        IsolatedJsModule = null;
        DotNetObjectRef = null;
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