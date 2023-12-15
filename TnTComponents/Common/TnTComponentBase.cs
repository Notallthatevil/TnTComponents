using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;
using TnTComponents.Common.Ext;

namespace TnTComponents.Common;

public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase, IAsyncDisposable {

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public virtual string? Class { get; set; }

    [Parameter]
    public string ComponentIdentifier { get; set; } = TnTComponentIdentifier.NewId();

    [Parameter]
    public virtual object? Data { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    [Parameter]
    public virtual string? Theme { get; set; }

    protected bool Interactive { get; set; }

    protected const string TnTCustomIdentifier = "tnt-custom-identifier";

    public virtual string GetClass() => this.GetClassDefault();

    protected virtual bool HasIsolatedJs { get; set; } = false;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _isolatedJsModule;

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            Interactive = true;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (HasIsolatedJs) {
            _isolatedJsModule ??= await JSRuntime.ImportIsolatedJs(this);
            await (_isolatedJsModule?.InvokeVoidAsync("onUpdate") ?? ValueTask.CompletedTask);
            int i = 0;
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? AdditionalAttributes.ToDictionary() : [];
        dict.TryAdd(TnTCustomIdentifier, ComponentIdentifier);
        AdditionalAttributes = dict;
    }

    public async ValueTask DisposeAsync() {
        try {
            if (_isolatedJsModule is not null) {
                await _isolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }
}