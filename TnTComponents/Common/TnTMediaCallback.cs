﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common {
    public  class TnTMediaCallback : ComponentBase, IDisposable {


        [Parameter]
        public string Query { get; set; } = default!;

        [Parameter]
        public EventCallback<bool> OnChangedCallback { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; } = default!;

        private DotNetObjectReference<TnTMediaCallback>? _reference;
        private bool disposedValue;

        [JSInvokable]
        public async Task OnChanged(bool queryIsTrue) {
            await OnChangedCallback.InvokeAsync(queryIsTrue);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if(firstRender) {
                _reference = DotNetObjectReference.Create(this);

                var result = await _jsRuntime.InvokeAsync<bool>("TnTComponents.MediaCallback", Query, _reference);

                await OnChangedCallback.InvokeAsync(result);
            }
        }
        
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {

                    if(_reference != null) {
                        _jsRuntime.InvokeVoidAsync("TnTComponents.MediaCallback", _reference);
                        _reference.Dispose();
                        _reference = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}