using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using TnTComponents.Ext;

namespace TnTComponents;

public partial class TnTLazyLoad {

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => throw new NotSupportedException();

    public override string? ElementStyle => throw new NotSupportedException();

    public override string? JsModulePath => _jsModulePath;

    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    private const string _jsModulePath = "./_content/TnTComponents/LazyLoader/TnTLazyLoad.razor.js";
    private bool _beginLoad;

    [DynamicDependency(nameof(BeginLoad))]
    public TnTLazyLoad() { }

    [JSInvokable]
    public void BeginLoad() {
        _beginLoad = true;
        StateHasChanged();
    }
}
