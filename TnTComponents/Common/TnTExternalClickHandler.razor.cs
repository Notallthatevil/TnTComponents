using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Common;

public partial class TnTExternalClickHandler {

    [Parameter, EditorRequired]
    public EventCallback Callback { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private ElementReference _externalClickElement;

    private DotNetObjectReference<TnTExternalClickHandler>? _reference;
    private bool _disposedValue;

    [JSInvokable]
    public async Task OnClick() {
        await Callback.InvokeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            _reference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("TnTComponents.WindowClickCallbackRegister", _externalClickElement, _reference);
        }
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing && _reference != null) {
                _jsRuntime.InvokeVoidAsync("TnTComponents.WindowClickCallbackDeregister", _reference);
                _reference.Dispose();
                _reference = null;
            }
            _disposedValue = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}