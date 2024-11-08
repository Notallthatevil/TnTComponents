using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Xml.Linq;
using TnTComponents.Ext;

namespace TnTComponents.LazyLoader;
public partial class TnTLazyLoad {

    public DotNetObjectReference<TnTLazyLoad>? DotNetObjectRef { get; set; }
    public IJSObjectReference? IsolatedJsModule { get; private set; }
    private const string _jsModulePath = "./_content/TnTComponents/LazyLoader/TnTLazyLoad.razor.js";

    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    private ElementReference _element;

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    private bool _beginLoad = false;

    public async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("onDispose", _element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        try {
            if (firstRender) {
                IsolatedJsModule = await JSRuntime.ImportIsolatedJs(this, _jsModulePath);
                await (IsolatedJsModule?.InvokeVoidAsync("onLoad", _element, DotNetObjectRef) ?? ValueTask.CompletedTask);
            }

            await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", _element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }
        catch (JSDisconnectedException) { }
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        DotNetObjectRef = DotNetObjectReference.Create(this);
    }

    [JSInvokable(nameof(BeginLoad))]
    public void BeginLoad() {
        _beginLoad = true;
        StateHasChanged();
    }
}
