using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Ext;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;

/// <summary>
///     Represents a base class for components that have an isolated JavaScript module.
/// </summary>
/// <typeparam name="TComponent">The type of the component.</typeparam>
public abstract class TnTPageScriptComponent<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent> : TnTComponentBase, ITnTPageScriptComponent<TComponent> where TComponent : ComponentBase {

    /// <inheritdoc />
    public DotNetObjectReference<TComponent>? DotNetObjectRef { get; set; }

    /// <inheritdoc />
    public IJSObjectReference? IsolatedJsModule { get; private set; }

    /// <inheritdoc />
    public abstract string? JsModulePath { get; }

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    /// <summary>
    ///     Gets the render fragment for the page script.
    /// </summary>
    protected RenderFragment PageScript;

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

    /// <inheritdoc />
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            DotNetObjectRef?.Dispose();
            DotNetObjectRef = null;

            if (IsolatedJsModule is IDisposable disposable) {
                disposable.Dispose();
                IsolatedJsModule = null;
            }
        }
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        var derived = this as TComponent;
        DotNetObjectRef = DotNetObjectReference.Create(derived!);
    }
}