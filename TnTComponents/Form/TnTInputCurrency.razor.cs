using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using TnTComponents.Ext;

namespace TnTComponents;
public partial class TnTInputCurrency<TCurrency> where TCurrency : struct, IFloatingPoint<TCurrency>, IParsable<TCurrency>  {

    private DotNetObjectReference<TnTInputCurrency<TCurrency>>? _dotNetObjRef;

    private IJSObjectReference? _isolatedJsModule;
    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;
    
    public override InputType Type => InputType.Text;

    private const string JsModulePath = "./_content/TnTComponents/Form/TnTInputCurrency.razor.js";

    protected override void OnInitialized() {
        base.OnInitialized();

        var dict = AdditionalAttributes is not null ? new Dictionary<string, object>(AdditionalAttributes) : [];
        dict.TryAdd("tnt-input-currency", "");
        dict.TryAdd("pattern", @"^\$?(([1-9](\d*|\d{0,2}(,(\d{1,3})?)*))|0)(\.(\d{1,2})?)?$");
        AdditionalAttributes = dict;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        _dotNetObjRef ??= DotNetObjectReference.Create(this);
        _isolatedJsModule ??= await _jsRuntime.ImportIsolatedJs(this, JsModulePath);
        if (firstRender) {
            await (_isolatedJsModule?.InvokeVoidAsync("onLoad", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
        }

        await (_isolatedJsModule?.InvokeVoidAsync("onUpdate", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TCurrency? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        validationErrorMessage = null;
        if (value is not null) {

            if (TCurrency.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var r)) {
                result = r;
                return true;
            }
            else if (value is null) {
                result = default;
                return true;
            }
            else {
                result = null;
                validationErrorMessage = $"Failed to parse {value} into a {typeof(decimal).Name}";
                return false;
            }
        }
        else {
            result = null;
            validationErrorMessage = null;
            return true;
        }

    }
}
