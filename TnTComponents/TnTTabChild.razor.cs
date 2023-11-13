using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Infrastructure;

namespace TnTComponents;
public partial class TnTTabChild {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public required string TabTitle { get; set; }

    internal ElementReference TabReference { get; set; }

    [CascadingParameter]
    private TabViewContext _tabViewContext { get; set; } = default!;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private bool _disabled;

    private bool _canGetSize;

    private ElementReference _reference;


    protected override void OnInitialized() {
        if (_tabViewContext is null) {
            throw new InvalidOperationException($"{nameof(TnTTabChild)} must be a descendant of {nameof(TnTTabView)}!");
        }

        _tabViewContext.AddChild(this);
        base.OnInitialized();
    }

    protected override void OnParametersSet() {
        if(_disabled != Disabled) {
            _disabled = Disabled;
            _tabViewContext?.ParentView.Refresh();
        }
        base.OnParametersSet();
    }

    protected override void OnAfterRender(bool firstRender) {
        _canGetSize = true;
        base.OnAfterRender(firstRender);
    }

    public void Dispose() {
        _tabViewContext.RemoveChild(this);
    }

    internal async Task<ElementOffset?> GetElementOffset() {
        if (_canGetSize) {
            return await _jsRuntime.GetElementOffset(_reference);
        }
        return default;
    }
}