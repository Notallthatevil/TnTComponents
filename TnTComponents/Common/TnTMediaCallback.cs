using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Common {

    public class TnTMediaCallback : ComponentBase, IDisposable {

        [Parameter]
        public EventCallback<bool> OnChangedCallback { get; set; }

        [Parameter]
        public string Query { get; set; } = default!;

        [Inject]
        private IJSRuntime _jsRuntime { get; set; } = default!;

        private bool _disposedValue;

        private DotNetObjectReference<TnTMediaCallback>? _reference;

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        [JSInvokable]
        public async Task OnChanged(bool queryIsTrue) {
            await OnChangedCallback.InvokeAsync(queryIsTrue);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    if (_reference != null) {
                        _jsRuntime.InvokeVoidAsync("TnTComponents.MediaCallback", _reference);
                        _reference.Dispose();
                        _reference = null;
                    }
                }
                _disposedValue = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                _reference = DotNetObjectReference.Create(this);

                var result = await _jsRuntime.InvokeAsync<bool>("TnTComponents.MediaCallback", Query, _reference);

                await OnChangedCallback.InvokeAsync(result);
            }
        }
    }
}