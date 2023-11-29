using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Infrastructure;

namespace TnTComponents;

public partial class TnTTabChild {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public string TabTitle { get; set; }

    internal ElementReference TabReference { get; set; }

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [CascadingParameter]
    private TabViewContext _tabViewContext { get; set; } = default!;

    private bool _canGetSize;

    private bool _disabled;

    private ElementReference _reference;

    public void Dispose() {
        _tabViewContext.RemoveChild(this);
    }

    internal async Task<ElementOffset?> GetElementOffset() {
        if (_canGetSize) {
            return await _jsRuntime.GetElementOffset(_reference);
        }
        return default;
    }

    protected override void OnAfterRender(bool firstRender) {
        _canGetSize = true;
        base.OnAfterRender(firstRender);
    }

    protected override void OnInitialized() {
        if (_tabViewContext is null) {
            throw new InvalidOperationException($"{nameof(TnTTabChild)} must be a descendant of {nameof(TnTTabView)}!");
        }

        _tabViewContext.AddChild(this);
        base.OnInitialized();
    }

    protected override void OnParametersSet() {
        if (_disabled != Disabled) {
            _disabled = Disabled;
            _tabViewContext?.ParentView.Refresh();
        }
        base.OnParametersSet();
    }
}