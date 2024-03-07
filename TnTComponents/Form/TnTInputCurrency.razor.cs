using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Xml.Linq;
using TnTComponents.Ext;

namespace TnTComponents;
public partial class TnTInputCurrency {
    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    [Parameter]
    public EventCallback<decimal?> BindAfter { get; set; }

    [Parameter]
    public bool BindOnInput { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter]
    public EventCallback<decimal?> OnChanged { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTColor? TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public decimal? Value { get; set; }

    private string? _currencyString;
    private string? CurrencyString {
        get => _currencyString;
        set {
            if (value is not null && decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var result)) {
                Value = result;
            }
            else {
                Value = null;
            }
            _currencyString = value;
        }
    }


    private DotNetObjectReference<TnTInputCurrency>? _dotNetObjRef;

    private IJSObjectReference? _isolatedJsModule;
    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private const string JsModulePath = "./_content/TnTComponents/Form/TnTInputCurrency.razor.js";

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        _dotNetObjRef ??= DotNetObjectReference.Create(this);
        _isolatedJsModule ??= await _jsRuntime.ImportIsolatedJs(this, JsModulePath);
        if (firstRender) {
            await (_isolatedJsModule?.InvokeVoidAsync("onLoad", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
        }

        await (_isolatedJsModule?.InvokeVoidAsync("onUpdate", null, _dotNetObjRef) ?? ValueTask.CompletedTask);
    }
}
