using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Common.Ext;

internal static class JSRuntimeExt {

    public static async Task<IJSObjectReference> Import(this IJSRuntime jsRuntime, string path) {
        return await jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
    }

    public static async Task<IJSObjectReference> ImportIsolatedJs(this IJSRuntime jsRuntime, object obj, string? path = null) {
        if (path is null) {

            var @namespace = obj.GetType().Namespace?.Split('.') ?? [];
            var name = obj.GetType().Name;
            if (name.Contains('`')) {
                name = name[..name.IndexOf('`')];
            }
            var root = Directory.GetCurrentDirectory();
            path = $"./_content/{string.Join('/', @namespace)}/{name}.razor.js";
        }
        return await jsRuntime.Import(path);
    }

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