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

    public static async Task<ElementBoundingRect> GetElementBoundingRect(this IJSRuntime jsRuntime, ElementReference element) {
        return await jsRuntime.InvokeAsync<ElementBoundingRect>("TnTComponents.getBoundingRect", element);
    }

    public static async Task<ElementOffset> GetElementOffset(this IJSRuntime jsRuntime, ElementReference element) {
        return await jsRuntime.InvokeAsync<ElementOffset>("TnTComponents.getOffsetPosition", element);
    }

    public static async Task<int> GetElementScrollPosition(this IJSRuntime jsRuntime, ElementReference element) {
        return await jsRuntime.InvokeAsync<int>("TnTComponents.getScrollPosition", element);
    }

    public static async Task<double> GetWindowHeight(this IJSRuntime jsRuntime) {
        return await jsRuntime.InvokeAsync<double>("TnTComponents.getWindowHeight");
    }
    public static async Task RemoveElementFocus(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.removeFocus", element);
    }

    public static async Task ScrollElementIntoView(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.scrollElementIntoView", element);
    }

    public static async Task SetElementBoundingRect(this IJSRuntime jsRuntime, ElementReference element, ElementBoundingRect boundingRect) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.setBoundingRect", element, boundingRect);
    }

    public static async Task SetElementFocus(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.setFocus", element);
    }

    public static async Task setElementScrollPosition(this IJSRuntime jsRuntime, ElementReference element, int position) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.setScrollPosition", element, position);
    }

}

