using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using TnTComponents.Ext;

namespace TnTComponents.Core;

/// <summary>
/// Base component containing all common logic.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
/// <seealso cref="TnTComponents.Core.ITnTComponentBase" />
/// <seealso cref="System.IAsyncDisposable" />
public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase, IAsyncDisposable {

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    public abstract string? Class { get; }

    public string ComponentIdentifier { get; } = TnTComponents.Core.TnTComponentIdentifier.NewId();

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    internal const string TnTCustomIdentifierAttribute = "tntid";

    protected DotNetObjectReference<TnTComponentBase>? DotNetObjectRef { get; set; }

    protected bool Interactive { get; private set; }

    protected IJSObjectReference? IsolatedJsModule { get; private set; }

    protected virtual string? JsModulePath => null;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected ILogger Logger => _logger ?? _loggerFactory.CreateLogger(GetType());

    /// <summary>
    /// If set, then this component expects an isolated js script to exist and contain onLoad,
    /// onUpdate, and onDispose.
    /// </summary>
    protected virtual bool RunIsolatedJsScript => false;

    [Inject]
    private ILoggerFactory _loggerFactory { get; set; } = default!;

    private ILogger? _logger;

    public async ValueTask DisposeAsync() {
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
            Interactive = true;
        }

        if (RunIsolatedJsScript) {
            DotNetObjectRef ??= DotNetObjectReference.Create(this);
            IsolatedJsModule ??= await JSRuntime.ImportIsolatedJs(this, JsModulePath);
            if (firstRender) {
                await (IsolatedJsModule?.InvokeVoidAsync("onLoad", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
            }

            await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? AdditionalAttributes.ToDictionary() : [];
        dict.TryAdd(TnTCustomIdentifierAttribute, ComponentIdentifier);
        AdditionalAttributes = dict;
    }
}