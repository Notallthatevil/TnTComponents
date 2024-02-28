using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents.Ext;

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





}