using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Common.Ext;

namespace TnTComponents.Core;

public partial class TnTExternalClickHandler {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => ExternalClickCssClass;

    [Parameter, EditorRequired]
    public EventCallback ExternalClickCallback { get; set; }

    [Parameter]
    public string? ExternalClickCssClass { get; set; }

    protected override string? JsModulePath => "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";
    protected override bool RunIsolatedJsScript => false;
    private IJSObjectReference? _jsModule;

    private DotNetObjectReference<TnTExternalClickHandler>? _reference;

    public new async ValueTask DisposeAsync() {
        try {
            if (_jsModule is not null) {
                await _jsModule.InvokeVoidAsync("externalClickCallbackDeregister", Element, _reference);
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }

        await base.DisposeAsync();
    }

    [JSInvokable]
    public async Task OnClick() {
        await ExternalClickCallback.InvokeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        _reference ??= DotNetObjectReference.Create(this);
        _jsModule ??= await JSRuntime.ImportIsolatedJs(this, JsModulePath);
        if (firstRender) {
            await _jsModule.InvokeVoidAsync("externalClickCallbackRegister", Element, _reference);
        }
    }
}