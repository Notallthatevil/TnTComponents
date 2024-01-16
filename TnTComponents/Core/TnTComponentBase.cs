using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.ComponentModel;
using TnTComponents.Common.Ext;

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

    [Inject]
    private ILoggerFactory _loggerFactory { get; set; } = default!;

    private ILogger? _logger;
    protected ILogger Logger => _logger ?? _loggerFactory.CreateLogger(GetType());

    public abstract string? Class { get; }

    public string ComponentIdentifier { get; } = TnTComponents.Core.TnComponentIdentifier.NewId();

    public ElementReference Element { get; protected set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    protected bool Interactive { get; private set; }
    protected IJSObjectReference? IsolatedJsModule { get; private set; }

    internal const string TnTCustomIdentifierAttribute = "tntid";

    /// <summary>
    /// If set, then this component expects an isolated js script to exist and contain onLoad, onUpdate, and onDispose.
    /// </summary>
    protected virtual bool RunIsolatedJsScript => false;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected DotNetObjectReference<TnTComponentBase>? DotNetObjectRef { get; set; }
    public bool? AutoFocus { get; set; }
    public bool? Disabled { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            Interactive = true;
        }

        if (RunIsolatedJsScript) {
            DotNetObjectRef ??= DotNetObjectReference.Create(this);
            IsolatedJsModule ??= await JSRuntime.ImportIsolatedJs(this);
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

    public async ValueTask DisposeAsync() {
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }
}