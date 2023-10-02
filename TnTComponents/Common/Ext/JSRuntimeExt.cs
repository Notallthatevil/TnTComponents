using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common.Ext;
internal static class JSRuntimeExt {
    public static async Task<double> ConvertRemToPixels(this IJSRuntime jsRuntime, double rem) {
        return await jsRuntime.InvokeAsync<double>("TnTComponents.remToPx", rem);
    }

    public static async Task<ElementBoundingRect> GetElementBoundingRectForComponent(this IJSRuntime jsRuntime, ElementReference element) {
        return await jsRuntime.InvokeAsync<ElementBoundingRect>("TnTComponents.getBoundingRect", element);
    }

    public static async Task<double> GetWindowHeight(this IJSRuntime jsRuntime) {
        return await jsRuntime.InvokeAsync<double>("TnTComponents.getWindowHeight");
    }
    public static async Task RemoveElementFocus(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.removeFocus", element);
    }

}

