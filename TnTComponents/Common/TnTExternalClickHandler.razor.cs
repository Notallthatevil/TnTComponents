using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Common;

public partial class TnTExternalClickHandler {
    private bool _disposedValue;

    private ElementReference _externalClickElement;

    private DotNetObjectReference<TnTExternalClickHandler>? _reference;

    [Parameter, EditorRequired]
    public EventCallback Callback { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    [JSInvokable]
    public async Task OnClick() {
        await Callback.InvokeAsync();
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

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            _reference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("TnTComponents.WindowClickCallbackRegister", _externalClickElement, _reference);
        }
    }
}