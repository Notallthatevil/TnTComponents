using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    //[Parameter]
    //public string TabHeaderClass { get; set; } = "tnt-tab-header";

    //[Parameter]
    //public string TabActiveIndicatorClass { get; set; } = "tnt-tab-active-indicator";

    //[Parameter]
    //public string TabContentClass { get; set; } = "tnt-tab-content";

    public override string? Class => CssBuilder.Create()
        .SetAlternative(SecondaryTabView)
        .Build();

    [Parameter]
    public bool SecondaryTabView { get; set; }

    [Parameter]
    public TnTColor HeaderBackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public TnTColor ActiveIndicatorColor { get; set; } = TnTColor.Primary;

    //public TnTTabChild? ActiveTab { get; private set; }

    protected override bool RunIsolatedJsScript => true;

    private List<TnTTabChild> _tabChildren = [];



    //protected override void OnAfterRender(bool firstRender) {
    //    base.OnAfterRender(firstRender);
    //    if (firstRender) {
    //        StateHasChanged();
    //    }
    //}

    //protected override async Task OnInitializedAsync() {
    //    await base.OnInitializedAsync();
    //    if (IsolatedJsModule is not null) {
    //        await IsolatedJsModule.InvokeVoidAsync("ensureInteractive", Element);
    //    }
    //}

    //protected override async Task OnAfterRenderAsync(bool firstRender) {
    //    await base.OnAfterRenderAsync(firstRender);
    //    if (firstRender) {
    //        await UpdateActiveIndicator();
    //    }
    //}

    //public async Task SetActiveTab(TnTTabChild? tabChild) {
    //    ActiveTab = tabChild;
    //    if (IsolatedJsModule is not null && ActiveTab is not null) {
    //        await IsolatedJsModule.InvokeVoidAsync("updateActiveIndex", Element, ActiveTab.Index);
    //    }
    //    await UpdateActiveIndicator();
    //    StateHasChanged();
    //}

    public void AddTabChild(TnTTabChild tabChild) {
        _tabChildren.Add(tabChild);
    }

    //public void RemoveTabChild(TnTTabChild tabChild) {
    //    _tabChildren.Remove(tabChild);
    //    var index = 0;
    //    foreach (var child in _tabChildren) {
    //        child.Index = index++;
    //    }
    //}

    //private async Task ButtonScrollListener(EventArgs _) {
    //    await UpdateActiveIndicator();
    //}

    //private async Task UpdateActiveIndicator() {
    //    if (IsolatedJsModule is not null) {
    //        await IsolatedJsModule.InvokeVoidAsync("updateActiveIndicator", Element, ActiveTab?.TabHeaderElement);
    //    }
    //}
}