using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents.Ext;

public static class JSRuntimeExt {

    public static async ValueTask DownloadFileFromStreamAsync(this IJSRuntime jsRuntime, Stream stream, string? fileName = null, CancellationToken cancellationToken = default) {
        using var streamRef = new DotNetStreamReference(stream);
        if (string.IsNullOrWhiteSpace(fileName)) {
            fileName = "download";
        }
        await jsRuntime.InvokeVoidAsync("TnTComponents.downloadFileFromStream", cancellationToken, fileName, streamRef);
    }

    public static async ValueTask DownloadFromUrlAsync(this IJSRuntime jsRuntime, string url, string? fileName = null, CancellationToken cancellationToken = default) {
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        if (string.IsNullOrWhiteSpace(fileName)) {
            fileName = "download";
        }
        await jsRuntime.InvokeVoidAsync("TnTComponents.downloadFromUrl", cancellationToken, fileName, url);
    }

    internal static async Task<BoundingClientRect?> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element) {
        return await jsRuntime.InvokeAsync<BoundingClientRect?>("TnTComponents.getBoundingClientRect", element);
    }

    internal static async ValueTask HideElement(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.hideElement", element);
    }

    internal static async Task<IJSObjectReference> Import(this IJSRuntime jsRuntime, string path) {
        return await jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
    }

    internal static async Task<IJSObjectReference> ImportIsolatedJs(this IJSRuntime jsRuntime, object obj, string? path = null) {
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

    internal static async ValueTask SetBoundingClientRectAsync(this IJSRuntime jsRuntime, ElementReference element, BoundingClientRect boundingClientRect) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.setBoundingClientRect", element, boundingClientRect);
    }

    internal static async ValueTask SetOpacity(this IJSRuntime jsRuntime, ElementReference element, float opacity) {
        opacity = Math.Clamp(opacity, 0, 1);
        await jsRuntime.InvokeVoidAsync("TnTComponents.setOpacity", element, opacity);
    }

    internal static async ValueTask ShowElementAsync(this IJSRuntime jsRuntime, ElementReference element) {
        await jsRuntime.InvokeVoidAsync("TnTComponents.showElement", element);
    }
}