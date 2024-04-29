using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Ext;

namespace TnTComponents;

public partial class TnTInputPhone {
    public override InputType Type => InputType.Tel;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private const string JsModulePath = "./_content/TnTComponents/Form/TnTInputPhone.razor.js";

    private DotNetObjectReference<TnTInputPhone>? _dotNetObjRef;

    private IJSObjectReference? _isolatedJsModule;

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        _dotNetObjRef ??= DotNetObjectReference.Create(this);
        _isolatedJsModule ??= await _jsRuntime.ImportIsolatedJs(this, JsModulePath);
        if (firstRender) {
            await (_isolatedJsModule?.InvokeVoidAsync("onLoad", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
        }

        await (_isolatedJsModule?.InvokeVoidAsync("onUpdate", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
    }

    protected override void OnInitialized() {
        base.OnInitialized();

        var dict = AdditionalAttributes is not null ? new Dictionary<string, object>(AdditionalAttributes) : [];
        dict.TryAdd("tnt-input-phone", "");
        dict.TryAdd("pattern", @"\(?[0-9]{1,3}\)? ?[0-9]{0,3}-?[0-9]{0,4}");
        AdditionalAttributes = dict;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}